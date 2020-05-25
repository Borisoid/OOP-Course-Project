using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.DataBase
{
    /// <summary>
    /// Returns SQL query for retrieving Gates that correspond to each of categories in List<string> Categories.
    /// If mentioned list is empty queries all gates.
    /// </summary>
    class CategoryQueryBilder : QueryBuilderBase
    {
        public CategoryQueryBilder()
        {
            Categories = new List<string>();
        }

        public List<string> Categories;

        
        public override string Query
        {
            get
            {
                if(Categories.Count == 0)
                {
                    return @"
                            SELECT DISTINCT
		                            Name,
		                            InputsNumber,
		                            Function
                            FROM view_Gates
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
