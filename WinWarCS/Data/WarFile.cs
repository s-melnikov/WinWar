// Author: Henning
// Project: WinWarEngine
// Path: P:\Projekte\WinWarCS\WinWarEngine\Data
// Creation date: 18.11.2009 10:13
// Last modified: 27.11.2009 10:10

#region Using directives
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data.Resources;
using System.Threading.Tasks;

#endregion
namespace WinWarCS.Data
{
   internal enum DataWarFileType
   {
      Unknown,
      Demo,
      Retail,
      RetailCD
   }

   internal static class WarFile
   {

      #region Members

      private static int fileID;    // 0x18 = Full; 0x19 = Demo
      private static int nrOfEntries;
      private static uint[] offsets;
      private static List<BasicResource> resources;
      /// <summary>
      /// The knowledge base for the currently loaded DATA.WAR
      /// Available after loading calling LoadResources()
      /// </summary>
      internal static KnowledgeBase KnowledgeBase;

      #endregion

      #region Properties

      public static DataWarFileType Type { get; private set; }

      public static bool IsDemo
      {
         get
         {
            return Type == DataWarFileType.Demo;
         }
      }

      public static bool HasIntroSpeech
      {
         get
         {
            return Type == DataWarFileType.RetailCD;
         }
      }

      #endregion

      #region LoadResources

      internal static async Task LoadResources()
      {
         Stream stream = null;
         BinaryReader reader = null;
         try
         {
            stream = await WinWarCS.Platform.IO.OpenContentFile("Assets" + Platform.IO.DirectorySeparatorChar + "Data" +
            Platform.IO.DirectorySeparatorChar + "DATA.WAR");

            reader = new BinaryReader(stream);

            fileID = reader.ReadInt32();							// ID
            nrOfEntries = reader.ReadInt32();		// Number of entries

            offsets = new uint[nrOfEntries];
            for (int i = 0; i < nrOfEntries; i++)
               offsets[i] = reader.ReadUInt32();

            Type = DataWarFileType.Retail;
            switch (fileID)
            {
            case 0x19:
               Type = DataWarFileType.Demo;
               break;
            case 0x18:
               Type = DataWarFileType.RetailCD;
               break;
            }

            // Create KnowledgeBase based on type of DATA.WAR
            KnowledgeBase = new KnowledgeBase(Type);

            // Load resources
            resources = new List<BasicResource>(nrOfEntries);
            ReadResources(reader);
         }
         catch (Exception ex)
         {
         }
         finally
         {
            if (reader != null)
               reader.Dispose();
            stream.Dispose();
         }
      }

      #endregion

      #region ReadResources

      private static int GetLength(BinaryReader br, int index)
      {
         if (offsets[index] == 0xFFFFFFFF)
            return 0;

         if (index == nrOfEntries - 1)
            return (int)br.BaseStream.Length - (int)offsets[index];

         int counter = 1;
         uint nextOffset = offsets[index + counter++];
         while (nextOffset == 0xFFFFFFFF)
         {
            if (index + counter >= offsets.Length)
            {
               nextOffset = (uint)br.BaseStream.Length;
               break;
            }

            nextOffset = offsets[index + counter++];
         }

         return (int)(nextOffset - offsets[index]);
      }

      private static BasicResource CreateResource(int index, ref List<WarResource> rawResources)
      {
         WarResource resource = rawResources[index];

         KnowledgeEntry ke = KnowledgeBase[index];

         ContentFileType fileType = ContentFileType.FileUnknown;
         if (ke != null)
            fileType = ke.type;

         switch (fileType)
         {
            case ContentFileType.FileBriefing:
               return new BasicResource(resource);

            case ContentFileType.FileCursor:
               return new BasicResource(resource);

            case ContentFileType.FileImage:
               return new BasicResource(resource);

            case ContentFileType.FileLevelInfo:
               return new LevelInfoResource(resource);

            case ContentFileType.FileLevelPassable:
               return new LevelPassableResource(resource);

            case ContentFileType.FileLevelVisual:
               return new LevelVisualResource(resource);

            case ContentFileType.FilePalette:
               return new PaletteResource(resource);

            case ContentFileType.FilePaletteShort:
               return new PaletteResource(resource);

            case ContentFileType.FileSprite:
               return new BasicResource(resource);

            case ContentFileType.FileTable:
               return new BasicResource(resource);

            case ContentFileType.FileText:
               return new BasicResource(resource);

            case ContentFileType.FileTiles:
               return new BasicResource(resource);

            case ContentFileType.FileTileSet:
               return new BasicResource(resource);

            case ContentFileType.FileUI:
               return new BasicResource(resource);

            case ContentFileType.FileVOC:
               return new BasicResource(resource);

            case ContentFileType.FileWave:
               return new BasicResource(resource);

            case ContentFileType.FileXMID:
               return new BasicResource(resource);

            default:
            case ContentFileType.FileUnknown:
               return new BasicResource(resource);
          }
      }

      private static int ReadResources(BinaryReader br)
      {
         List<WarResource> rawResources = new List<WarResource>();

         int result = 0;
         for (int i = 0; i < nrOfEntries; i++)
         {
            // Happens with demo data
            if (offsets[i] == 0xFFFFFFFF)
            {
               resources.Add(null);
               continue;
            }

            br.BaseStream.Seek((long)offsets[i], SeekOrigin.Begin);

            int compr_length = GetLength(br, i);
            long offset = br.BaseStream.Position;

            rawResources.Add(new WarResource(br, offset, compr_length, i));

            result++;
         }

         // Build resources
         for (int i = 0; i < nrOfEntries; i++)
         {
            // Happens with demo data
            if (rawResources[i] == null)
            {
               resources.Add(null);
               continue;
            }

            resources.Add(CreateResource(i, ref rawResources));
         }

         return result;
      }

      private static void WriteResource(BinaryWriter writer, WarResource res)
      {
         writer.Write((ushort)res.data.Length);
         writer.Write((byte)0);

         byte comprFlag = 0;
         writer.Write(comprFlag);

         writer.Write(res.data);
      }

      #endregion

      #region WriteWarFile
      private static void WriteWarFile(string outputFile)
      {
#if !NETFX_CORE
         using (BinaryWriter writer = new BinaryWriter(File.Create(outputFile)))
         {
            writer.Write(fileID);
            writer.Write(resources.Count);

            uint offsetOfOffsetTable = (uint)writer.BaseStream.Position;
            // Write empty offset table
            for (int i = 0; i < resources.Count; i++)
               writer.Write((int)0);

            uint curOffset = (uint)writer.BaseStream.Position;

            // Write each resource and remember offset
            for (int i = 0; i < resources.Count; i++)
            {
               if (resources[i] == null)
               {
                  offsets[i] = 0xFFFFFFFF;
                  continue;
               }

               offsets[i] = curOffset;

               WriteResource(writer, resources[i].Resource);

               curOffset = (uint)writer.BaseStream.Position;
            }

            // Write full offset table
            writer.BaseStream.Seek(offsetOfOffsetTable, SeekOrigin.Begin);
            for (int i = 0; i < resources.Count; i++)
               writer.Write(offsets[i]);
         }
 #endif
      }
      #endregion

      #region DumpResources

      #if !NETFX_CORE
      internal static void DumpResources(string path)
      {
         for (int i = 0; i < resources.Count; i++)
         {
            ContentFileType fileType = ContentFileType.FileUnknown;
            if (i < KnowledgeBase.Count)
               fileType = KnowledgeBase[i].type;
            string filename = Path.Combine(path, "res" + i + "." + fileType);
            File.WriteAllBytes(filename, resources[i].Resource.data);
         }
      }
      #endif
      #endregion

      #region GetImageResource

      internal static ImageResource GetImageResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileImage)
            return null;

         return new ImageResource(GetResource(id), GetResource(KnowledgeBase[id].param));
      }

      #endregion

      #region GetCursorResource

      internal static CursorResource GetCursorResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileCursor)
            return null;

         WarResource pal = null;
         if (KnowledgeBase[KnowledgeBase[id].param].type == ContentFileType.FilePalette)
            pal = GetResource(KnowledgeBase[id].param);

         return new CursorResource(GetResource(id), pal);
      }

      #endregion

      #region GetSpriteResource

      internal static SpriteResource GetSpriteResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileSprite)
            return null;

         WarResource pal = null;
         if (KnowledgeBase[KnowledgeBase[id].param].type == ContentFileType.FilePalette)
            pal = GetResource(KnowledgeBase[id].param);

         return new SpriteResource(GetResource(id), pal);
      }

      #endregion

      #region GetUIResource

      internal static UIResource GetUIResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileUI)
            return null;

         return new UIResource(GetResource(id));
      }

      #endregion

      #region GetResource

      internal static WarResource GetResource(int index)
      {
         if ((index < 0) || (index >= Count))
            return null;

         return resources[index].Resource;
      }

      #endregion

      #region GetResourceByName

      /// <summary>
      /// Returns the resource using a hash table indexed by the names
      /// </summary>
      /// <param name="name">Name of the resource</param>
      /// <returns>The resource or null if no resource of the given name exists</returns>
      internal static WarResource GetResourceByName(string name)
      {
         int idx = KnowledgeBase.IndexByName(name);
         if (idx == -1)
            return null;

         return resources[idx].Resource;
      }

      #endregion

      #region Properties

      internal static int Count
      {
         get
         {
            if (resources == null)
               return 0;

            return resources.Count; 
         }
      }

      #endregion

   }
}
