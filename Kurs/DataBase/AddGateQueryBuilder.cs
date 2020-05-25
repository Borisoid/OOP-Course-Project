using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs.DataBase
{
    class AddGateQueryBuilder : QueryBuilderBase
    {
        public AddGateQueryBuilder()
        { }

        public AddGateQueryBuilder(string gateName, int inputsNumber, string function, List<string> categories)
        {
            GateName = gateName;
            InputsNumber = inputsNumber;
            Function = function;
            Categories = new List<string>(categories);
        }

        public string GateName;
        public int InputsNumber;
        public string Function;
        public List<string> Categories;

        public override string Query
        {
            get
            {
                string q = @$"INSERT INTO tbl_Gates(Name, InputsNumber, Function) VALUES
                            ('{GateName}', {InputsNumber}, '{Function}');";

                foreach (string category in Categories)
                {
                    q = q + @$"INSERT INTO tbl_Relations(GateID, CategoryID) VALUES
                            (
                                (SELECT ID FROM tbl_gates WHERE Name = '{GateName}' LIMIT 1),
                                (SELECT ID FROM tbl_Categories WHERE Name = '{category}' LIMIT 1)
                            );";
                }

                return q;
            }
        }
    }
}
