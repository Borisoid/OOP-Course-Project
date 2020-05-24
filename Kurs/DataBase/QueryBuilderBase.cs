using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.DataBase
{
    abstract class QueryBuilderBase
    {
        /// <summary>
        /// Query built accordingly to builder's settings.
        /// </summary>
        public abstract string Query { get; }
    }
}
