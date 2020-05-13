using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.DataBase
{
    class QueryBilder
    {
        public QueryBilder()
        {
            Categories = new List<string>();
        }

        public List<string> Categories;

        public string Query
        {
            get
            {
                if(Categories.Count == 0)
                {
                    return @"
                            SELECT DISTINCT
		                            GateName,
		                            InputsNumber,
		                            Function
                            FROM view_CategoryDivision
                            ";
                }


                string q = @"
                            SELECT DISTINCT
	                        *
                            FROM
                            (
                            ";
                foreach(string category in Categories)
                {
                    q += @"
                          select
		                    GateName,
		                    InputsNumber,
		                    Function
	                      FROM view_CategoryDivision where CategoryName = " + $"\'{category}\'";
                    //if current item isn't last add 'INTERSECT'
                    if (Categories.IndexOf(category) != Categories.Count - 1)
                        q += "\nINTERSECT\n";
                }
                q += ")";

                return q;
            }
        }
    }
}
