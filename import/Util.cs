﻿using System;
using System.IO;
using System.Drawing;
using System.Globalization;

namespace import
{
	class Util
	{
		static public int ReadInt24(BinaryReader br)
		{
			byte b1 = br.ReadByte();
			byte b2 = br.ReadByte();
			byte b3 = br.ReadByte();
			return (b1 << 16) | (b2 << 8) | (b3);
		}

		static public int ReadInt32(BinaryReader br)
		{
			byte[] r;
			r = new byte[4];
			for (int i = 0; i < 4; i++)
				r[3 - i] = br.ReadByte();
			return BitConverter.ToInt32(r, 0);
		}

		static public byte Nibble4(byte val, bool low)
		{
			if (low)
				return (byte)(val & 0x0F);
			else
				return (byte)((val >> 4) & 0x0F);
		}

		static public int ModNeg(int x, int y)
		{
			if (x < 0)
				return (y - 1) - (Math.Abs(x + 1) % y);
			else
				return x % y;
		}

		static public int IntDiv(int a, int b)
		{
			return (int)Math.Floor(((decimal)a) / ((decimal)b));
		}

		static public Color ColorAdd(Color a, Color b)
		{
			float bA = ((float)b.A) / 255.0f;
			float bAinv = (1.0f - bA);
			return Color.FromArgb(
				Math.Min(255, a.A + b.A),
				(int)Math.Min(255, a.R * bAinv + b.R * bA),
				(int)Math.Min(255, a.G * bAinv + b.G * bA),
				(int)Math.Min(255, a.B * bAinv + b.B * bA)
			);
		}

		static public Color ColorBrighter(Color from, int add)
		{
			return Color.FromArgb(
				from.A,
				(int)(Math.Max(0, Math.Min(255, from.R + add))),
				(int)(Math.Max(0, Math.Min(255, from.G + add))),
				(int)(Math.Max(0, Math.Min(255, from.B + add)))
			);
		}

		public static Color HexToColor(string hex, int alpha = 255)
		{
			Color col = Color.FromArgb(Int32.Parse(hex.Replace("#", ""), NumberStyles.HexNumber));
			return Color.FromArgb(alpha, col.R, col.G, col.B);
		}
	}
}