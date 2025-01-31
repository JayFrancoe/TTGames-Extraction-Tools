﻿// Decompiled with JetBrains decompiler
// Type: ExtractDx11MESH.MESHs.MESH2E
// Assembly: ExtractDx11MESH, Version=1.0.8142.30912, Culture=neutral, PublicKeyToken=null
// MVID: 29BAC945-368A-4795-B445-7620ADD62E50
// Assembly location: D:\LegoExtracted\tools\DX11ExtractNew\ExtractDx11MESH.exe

using ExtractHelper;
using ExtractHelper.VariableTypes;
using System;
using System.Collections.Generic;

namespace ExtractDx11MESH.MESHs
{
  public class MESH2E : MESH05
  {
    public MESH2E(byte[] fileData, int iPos)
      : base(fileData, iPos)
    {
    }

    public override int Read(ref int referencecounter)
    {
      int int32 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}   Number of Parts: 0x{1:x8}", (object) this.iPos, (object) int32);
      this.iPos += 4;
      for (int index = 0; index < int32; ++index)
      {
        ColoredConsole.WriteLine("{0:x8}   Part 0x{1:x8}", (object) this.iPos, (object) index);
        this.Parts.Add(this.ReadPart(ref referencecounter));
      }
      return this.iPos;
    }

    protected override Part ReadPart(ref int referencecounter)
    {
      Part part = new Part();
      int int32_1 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}     Number of Vertex Lists: 0x{1:x8}", (object) this.iPos, (object) int32_1);
      this.iPos += 4;
      for (int index = 0; index < int32_1; ++index)
      {
        ColoredConsole.WriteLine("{0:x8}       Vertex List 0x{1:x8}", (object) this.iPos, (object) index);
        part.VertexListReferences1.Add(this.GetVertexListReference(ref referencecounter));
      }
      ColoredConsole.WriteLine("{0:x8}           Unknown Number of Index Lists ? 0x{1:x8}", (object) this.iPos, (object) BigEndianBitConverter.ToInt32(this.fileData, this.iPos));
      this.iPos += 4;
      part.IndexListReference1 = this.GetIndexListReference(ref referencecounter);
      part.OffsetIndices = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}     Offset Indices: 0x{1:x8}", (object) this.iPos, (object) part.OffsetIndices);
      this.iPos += 4;
      part.NumberIndices = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}     Number Indices: 0x{1:x8}", (object) this.iPos, (object) part.NumberIndices);
      this.iPos += 4;
      part.OffsetVertices = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}     Offset Vertices: 0x{1:x8}", (object) this.iPos, (object) part.OffsetVertices);
      this.iPos += 4;
      if (BigEndianBitConverter.ToInt16(this.fileData, this.iPos) != (short) 0)
        throw new NotSupportedException("ReadPart Offset Vertices + 4");
      this.iPos += 2;
      part.NumberVertices = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}     Number Vertices: 0x{1:x8}", (object) this.iPos, (object) part.NumberVertices);
      this.iPos += 4;
      ++referencecounter;
      this.iPos += 4;
      int int32_2 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      this.iPos += 4;
      if (int32_2 > 0)
      {
        ColoredConsole.Write("{0:x8}     ", (object) this.iPos);
        for (int index = 0; index < int32_2; ++index)
        {
          ColoredConsole.Write("{0:x2} ", (object) this.fileData[this.iPos]);
          ++this.iPos;
        }
        ColoredConsole.WriteLine();
        ++referencecounter;
      }
      int int32_3 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      this.iPos += 4;
      if (int32_3 != 0)
      {
        int num = this.ReadRelativePositionList();
        referencecounter += num;
      }
      this.iPos += 4;
      this.iPos += 36;
      return part;
    }

    protected override int ReadRelativePositionList()
    {
      this.iPos += 4;
      int num1 = 1;
      int num2 = 0;
      while (BigEndianBitConverter.ToInt32(this.fileData, this.iPos) != 0)
      {
        this.iPos += 8;
        ++num1;
      }
      ColoredConsole.WriteLine("{0:x8}     Relative Position Lists: 0x{1:x8}", (object) this.iPos, (object) num1);
      this.iPos += 4;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int int32_1 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
        this.iPos += 4;
        if (int32_1 == 0)
        {
          this.iPos += 5;
          this.iPos += 4;
          this.iPos += 4;
          int int32_2 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
          ColoredConsole.WriteLine("{0:x8}       Size of Relative Positions: 0x{1:x8}", (object) this.iPos, (object) int32_2);
          this.iPos += 4;
          this.iPos += int32_2;
          int num3 = num2 + 1;
          int int32_3 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
          ColoredConsole.WriteLine("{0:x8}       Relative Position Tupels: 0x{1:x8}", (object) this.iPos, (object) int32_3);
          this.iPos += 4;
          this.iPos += 4 * int32_3;
          if (int32_3 > 0)
            ++num3;
          num2 = num3 + 1;
        }
        else
        {
          num2 = num2 + 1 + 1;
          for (int index2 = 0; index2 < int32_1; ++index2)
            this.iPos += 12;
          this.iPos += 21;
        }
      }
      return num2;
    }

    protected override int GetIndexListReference(ref int referencecounter)
    {
      int indexListReference;
      if (this.fileData[this.iPos] == (byte) 192)
      {
        indexListReference = (int) BigEndianBitConverter.ToInt16(this.fileData, this.iPos + 2);
        this.iPos += 4;
        ColoredConsole.WriteLine("{0:x8}     Index List Reference to 0x{1:x4}", (object) this.iPos, (object) indexListReference);
        this.iPos += 4;
      }
      else
      {
        ColoredConsole.WriteLine("{0:x8}         New Index List 0x{1:x4}", (object) this.iPos, (object) referencecounter);
        ColoredConsole.WriteLine("{0:x8}           Unknown 0x{1:x8}", (object) this.iPos, (object) BigEndianBitConverter.ToInt32(this.fileData, this.iPos));
        this.iPos += 4;
        int int32_1 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
        ColoredConsole.WriteLine("{0:x8}           Unknown 0x{1:x8}", (object) this.iPos, (object) int32_1);
        this.iPos += 4;
        if (int32_1 == 4354)
          ColoredConsole.WriteWarn("New unknown Index");
        int int32_2 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
        ColoredConsole.WriteLine("{0:x8}           Number of Indices: {1:x8}", (object) this.iPos, (object) int32_2);
        this.iPos += 4;
        int int32_3 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
        ColoredConsole.WriteLine("{0:x8}           IndexSize 0x{1:x8}", (object) this.iPos, (object) int32_3);
        this.iPos += 4;
        List<int> intList = new List<int>();
        for (int index = 0; index < int32_2; ++index)
        {
          switch (int32_3)
          {
            case 2:
              intList.Add((int) BitConverter.ToUInt16(this.fileData, this.iPos));
              this.iPos += 2;
              break;
            case 4:
              intList.Add(BitConverter.ToInt32(this.fileData, this.iPos));
              this.iPos += 4;
              break;
          }
        }
        this.Indexlistsdictionary.Add(referencecounter, intList);
        indexListReference = referencecounter++;
      }
      return indexListReference;
    }

    protected override int GetVertexListReference(ref int referencecounter)
    {
      int vertexListReference;
      if (this.fileData[this.iPos] == (byte) 192)
      {
        vertexListReference = (int) BigEndianBitConverter.ToInt16(this.fileData, this.iPos + 2);
        this.iPos += 4;
        ColoredConsole.WriteLineWarn("{0:x8}         Vertex List Reference to 0x{1:x4}", (object) this.iPos, (object) vertexListReference);
        this.iPos += 4;
        this.iPos += 4;
      }
      else
      {
        ColoredConsole.WriteLineWarn("{0:x8}         New Vertex List 0x{1:x4}", (object) this.iPos, (object) referencecounter);
        this.iPos += 4;
        this.iPos += 4;
        int int32 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
        this.iPos += 4;
        VertexList vertexList = this.ReadVertexList(int32);
        this.iPos += 4;
        this.Vertexlistsdictionary.Add(referencecounter, vertexList);
        vertexListReference = referencecounter++;
      }
      return vertexListReference;
    }

    protected override VertexList ReadVertexList(int numberofvertices)
    {
      VertexList vertexList = new VertexList();
      int int32 = BigEndianBitConverter.ToInt32(this.fileData, this.iPos);
      ColoredConsole.WriteLine("{0:x8}           Number of Vertex Definitions: {1:x8}", (object) this.iPos, (object) int32);
      this.iPos += 4;
      for (int index = 0; index < int32; ++index)
        vertexList.VertexDefinitions.Add(this.ReadVertexDefinition());
      ColoredConsole.WriteLine("{0:x8}           Number of Vertices: {1:x8}", (object) this.iPos, (object) numberofvertices);
      for (int index = 0; index < numberofvertices; ++index)
        vertexList.Vertices.Add(this.ReadVertex(vertexList.VertexDefinitions));
      return vertexList;
    }

    protected virtual VertexDefinition ReadVertexDefinition()
    {
      VertexDefinition vertexDefinition = new VertexDefinition();
      vertexDefinition.Variable = (VertexDefinition.VariableEnum) this.fileData[this.iPos];
      vertexDefinition.VariableType = (VertexDefinition.VariableTypeEnum) this.fileData[this.iPos + 1];
      vertexDefinition.Offset = (int) this.fileData[this.iPos + 2];
      ColoredConsole.WriteLine("{0:x8}             {1} {2}", (object) this.iPos, (object) vertexDefinition.VariableType.ToString(), (object) vertexDefinition.Variable.ToString());
      this.iPos += 3;
      return vertexDefinition;
    }

    protected override Vertex ReadVertex(List<VertexDefinition> vertexdefinitions)
    {
      Vertex vertex = new Vertex();
      foreach (VertexDefinition vertexdefinition in vertexdefinitions)
      {
        switch (vertexdefinition.Variable)
        {
          case VertexDefinition.VariableEnum.position:
            vertex.Position = (Vector3) this.ReadVariableValue(vertexdefinition.VariableType);
            break;
          case VertexDefinition.VariableEnum.normal:
            vertex.Normal = (Vector3) this.ReadVariableValue(vertexdefinition.VariableType);
            break;
          case VertexDefinition.VariableEnum.colorSet0:
            vertex.ColorSet0 = (Color4) this.ReadVariableValue(vertexdefinition.VariableType);
            break;
          case VertexDefinition.VariableEnum.tangent:
          case VertexDefinition.VariableEnum.unknown6:
          case VertexDefinition.VariableEnum.uvSet2:
          case VertexDefinition.VariableEnum.unknown8:
          case VertexDefinition.VariableEnum.blendIndices0:
          case VertexDefinition.VariableEnum.blendWeight0:
          case VertexDefinition.VariableEnum.unknown11:
          case VertexDefinition.VariableEnum.lightDirSet:
          case VertexDefinition.VariableEnum.lightColSet:
            this.ReadVariableValue(vertexdefinition.VariableType);
            break;
          case VertexDefinition.VariableEnum.colorSet1:
            vertex.ColorSet1 = (Color4) this.ReadVariableValue(vertexdefinition.VariableType);
            break;
          case VertexDefinition.VariableEnum.uvSet01:
            vertex.UVSet0 = (Vector2) this.ReadVariableValue(vertexdefinition.VariableType);
            break;
          default:
            this.ReadVariableValue(vertexdefinition.VariableType);
            break;
        }
      }
      return vertex;
    }

    protected override object ReadVariableValue(VertexDefinition.VariableTypeEnum variabletype)
    {
      switch (variabletype)
      {
        case VertexDefinition.VariableTypeEnum.vec2float:
          Vector2 vector2_1 = new Vector2()
          {
            X = BitConverter.ToSingle(this.fileData, this.iPos),
            Y = BitConverter.ToSingle(this.fileData, this.iPos + 4)
          };
          this.iPos += 8;
          return (object) vector2_1;
        case VertexDefinition.VariableTypeEnum.vec3float:
          Vector3 vector3_1 = new Vector3();
          vector3_1.X = BitConverter.ToSingle(this.fileData, this.iPos);
          vector3_1.Y = BitConverter.ToSingle(this.fileData, this.iPos + 4);
          vector3_1.Z = BitConverter.ToSingle(this.fileData, this.iPos + 8);
          Vector3 vector3_2 = vector3_1;
          this.iPos += 12;
          return (object) vector3_2;
        case VertexDefinition.VariableTypeEnum.vec4float:
          Vector4 vector4_1 = new Vector4();
          vector4_1.X = BitConverter.ToSingle(this.fileData, this.iPos);
          vector4_1.Y = BitConverter.ToSingle(this.fileData, this.iPos + 4);
          vector4_1.Z = BitConverter.ToSingle(this.fileData, this.iPos + 8);
          vector4_1.W = BitConverter.ToSingle(this.fileData, this.iPos + 12);
          Vector4 vector4_2 = vector4_1;
          this.iPos += 16;
          return (object) vector4_2;
        case VertexDefinition.VariableTypeEnum.vec2half:
          Vector2 vector2_2 = new Vector2()
          {
            X = (float) Half.ToHalf(this.fileData, this.iPos),
            Y = (float) Half.ToHalf(this.fileData, this.iPos + 2)
          };
          this.iPos += 4;
          return (object) vector2_2;
        case VertexDefinition.VariableTypeEnum.vec4half:
          Vector4 vector4_3 = new Vector4();
          vector4_3.X = (float) Half.ToHalf(this.fileData, this.iPos);
          vector4_3.Y = (float) Half.ToHalf(this.fileData, this.iPos + 2);
          vector4_3.Z = (float) Half.ToHalf(this.fileData, this.iPos + 4);
          vector4_3.W = (float) Half.ToHalf(this.fileData, this.iPos + 6);
          Vector4 vector4_4 = vector4_3;
          this.iPos += 8;
          return (object) vector4_4;
        case VertexDefinition.VariableTypeEnum.vec4char:
          this.iPos += 4;
          return (object) 1;
        case VertexDefinition.VariableTypeEnum.vec4mini:
          Vector4 vector4_5 = new Vector4();
          vector4_5.X = this.LookUp[(int) this.fileData[this.iPos]];
          vector4_5.Y = this.LookUp[(int) this.fileData[this.iPos + 1]];
          vector4_5.Z = this.LookUp[(int) this.fileData[this.iPos + 2]];
          vector4_5.W = this.LookUp[(int) this.fileData[this.iPos + 3]];
          Vector4 vector4_6 = vector4_5;
          this.iPos += 4;
          return (object) vector4_6;
        case VertexDefinition.VariableTypeEnum.color4char:
          Color4 color4 = new Color4()
          {
            B = (int) this.fileData[this.iPos],
            G = (int) this.fileData[this.iPos + 1],
            R = (int) this.fileData[this.iPos + 2],
            A = (int) this.fileData[this.iPos + 3]
          };
          this.iPos += 4;
          return (object) color4;
        default:
          throw new NotImplementedException(variabletype.ToString());
      }
    }
  }
}
