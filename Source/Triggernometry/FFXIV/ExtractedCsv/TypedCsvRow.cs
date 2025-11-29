using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Triggernometry.Utilities;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    public abstract class TypedCsvRow : CsvRow
    {
        public abstract string Name { get; }

        public TRow GetRow<TRow>(int id) where TRow : TypedCsvRow
        {
            return Manager.Get<TRow>()[id];
        }

    }

}
