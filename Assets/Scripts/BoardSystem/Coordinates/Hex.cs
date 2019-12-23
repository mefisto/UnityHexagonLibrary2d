﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HexCardGame.Runtime
{
    public class SortHexAscendant : IComparer<Hex>
    {
        public int Compare(Hex x, Hex y) => x.CompareTo(y);
    }

    public class SortHexDescendant : IComparer<Hex>
    {
        public int Compare(Hex x, Hex y) => y.CompareTo(x);
    }

    public struct Hex : IComparable
    {
        static readonly Hex[] Diagonals =
        {
            new Hex(1, -2, 1),
            new Hex(-1, -1, 2),
            new Hex(-2, 1, 1),
            new Hex(-1, 2, -1),
            new Hex(1, 1, -2),
            new Hex(2, -1, -1)
        };

        static readonly Hex[] Directions =
        {
            new Hex(1, -1, 0),
            new Hex(0, -1, 1),
            new Hex(-1, 0, 1),
            new Hex(-1, 1, 0),
            new Hex(0, 1, -1),
            new Hex(1, 0, -1)
        };

        public int q { get; } //column, X axis
        public int r { get; } //row, Y axis
        public int s { get; }

        public Hex(int q, int r)
        {
            this.q = q;
            this.r = r;
            s = -(q + r);
            Assert.AreEqual(0, s + q + r);
        }

        public Hex(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public AxialCoord ToAxialCoord() => new AxialCoord(q, r);

        public OffsetCoord ToQoffsetEven() => OffsetCoordHelper.QoffsetFromCube(OffsetCoord.Parity.Even, this);
        public OffsetCoord ToQoffsetOdd() => OffsetCoordHelper.QoffsetFromCube(OffsetCoord.Parity.Odd, this);
        public OffsetCoord ToRoffsetEven() => OffsetCoordHelper.RoffsetFromCube(OffsetCoord.Parity.Even, this);
        public OffsetCoord ToRoffsetOdd() => OffsetCoordHelper.RoffsetFromCube(OffsetCoord.Parity.Odd, this);

        public int Length => (Mathf.Abs(q) + Mathf.Abs(r) + Mathf.Abs(s)) / 2;
        public static bool operator ==(Hex a, Hex b) => a.q == b.q && a.r == b.r && a.s == b.s;
        public static bool operator !=(Hex a, Hex b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var other = (Hex) obj;
            return q == other.q && r == other.r;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + q.GetHashCode();
                hash = hash * 23 + r.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(object obj)
        {
            var val = (Hex) obj;
            if (Equals(obj))
                return 0;

            var xComparison = q > val.q;
            var yComparison = r > val.r;

            if (xComparison)
                return 1;
            if (yComparison)
                return 1;

            return -1;
        }

        public override string ToString() => $"Hex: ({q}, {r}, {s})";

        #region Operators

        public static Hex Add(Hex a, Hex b) => new Hex(a.q + b.q, a.r + b.r);
        public static Hex Subtract(Hex a, Hex b) => new Hex(a.q - b.q, a.r - b.r);
        public static Hex Multiply(Hex a, int k) => new Hex(a.q * k, a.r * k);
        public static int Distance(Hex a, Hex b) => Subtract(a, b).Length;

        #endregion
    }
}