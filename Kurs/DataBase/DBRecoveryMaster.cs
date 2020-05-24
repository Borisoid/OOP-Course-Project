using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Kurs.DataBase
{
    static class DBRecoveryMaster
    {
        public static void CheckConnection(SQLiteConnection con)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='tbl_Gates';";
            if (cmd.ExecuteScalar() == null)
                Recover(con);
        }

        public static void Recover(SQLiteConnection con)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "DROP Table IF EXISTS tbl_Gates;";
            cmd.CommandText += "DROP Table IF EXISTS tbl_Categories;";
            cmd.CommandText += "DROP Table IF EXISTS tbl_Relations;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE 'tbl_Gates' (
                                'ID'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                            'Name'  TEXT NOT NULL DEFAULT 'NoName' UNIQUE,
	                            'InputsNumber'  INTEGER NOT NULL CHECK(InputsNumber >= 0),
	                            'Function'  TEXT NOT NULL
                                );";
            cmd.CommandText += @"CREATE TABLE 'tbl_Categories' (
                              'ID'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                          'Name'  TEXT NOT NULL UNIQUE
                              );";
            cmd.CommandText += @"CREATE TABLE 'tbl_Relations' (
                                'ID'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                            'GateID'    INTEGER NOT NULL,
	                            'CategoryID'    INTEGER NOT NULL,
	                            FOREIGN KEY('GateID') REFERENCES 'tbl_Gates'('ID'),
	                            FOREIGN KEY('CategoryID') REFERENCES 'tbl_Categories'('ID')
                                );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE VIEW IF NOT EXISTS view_Gates AS
	                            SELECT
		                            Name,
		                            InputsNumber,
		                            Function
	                            FROM
		                            tbl_Gates;";
            cmd.CommandText += @"CREATE VIEW IF NOT EXISTS view_Categories AS
	                            SELECT
		                            Name
	                            FROM
		                            tbl_Categories;";
            cmd.CommandText += @"CREATE VIEW IF NOT EXISTS view_CategoryDivision as
	                            SELECT
		                            tbl_Gates.ID as GateID,
		                            tbl_Gates.Name as GateName,
		                            tbl_Gates.InputsNumber,
		                            tbl_Gates.Function,
		                            tbl_Categories.ID as CategoryID,
		                            tbl_Categories.Name as CategoryName
	                            FROM
		                            tbl_Gates inner join
		                            tbl_Relations on tbl_Relations.GateID = tbl_Gates.ID inner join
		                            tbl_Categories on tbl_Categories.ID = tbl_Relations.CategoryID;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT INTO tbl_Gates(Name, InputsNumber, Function)
                                VALUES
                                ('SOURCE', 0, '0'),
                                ('READER', 1, '10'),
                                ('NOT', 1, '01'),
                                ('2AND', 2, '1000'),
                                ('2OR', 2, '1110'),
                                ('2XOR', 2, '0110'),
                                ('2NAND', 2, '0111'),
                                ('2NOR', 2, '0001'),
                                ('2XNOR', 2, '1001'),
                                ('3AND', 3, '10000000'),
                                ('3OR', 3, '11111110'),
                                ('3XOR', 3, '00010110'),
                                ('3NAND', 3, '01111111'),
                                ('3NOR', 3, '00000001'),
                                ('3XNOR', 3, '11101001');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT INTO tbl_Categories(Name)
                                VALUES
                                ('NATIVE'),
                                ('PERIPHERAL'),
                                ('NOT'),
                                ('EXCLUSIVE'),
                                ('AND'),
                                ('OR'),
                                ('2INPUTS'),
                                ('3INPUTS');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT INTO tbl_Relations(GateID,CategoryID)
                                VALUES
                                (1,1),
                                (2,1),
                                (3, 1),
                                (4, 1),
                                (5, 1),
                                (6, 1),
                                (7, 1),
                                (8, 1),
                                (9, 1),
                                (10, 1),
                                (11, 1),
                                (12, 1),
                                (13, 1),
                                (14, 1),
                                (15, 1),
                                (1, 2),
                                (2, 2),
                                (3, 3),
                                (7, 3),
                                (8, 3),
                                (9, 3),
                                (13, 3),
                                (14, 3),
                                (15, 3),
                                (6, 4),
                                (9, 4),
                                (12, 4),
                                (15, 4),
                                (4, 5),
                                (7, 5),
                                (10, 5),
                                (13, 5),
                                (5, 6),
                                (6, 6),
                                (8, 6),
                                (9, 6),
                                (11, 6),
                                (12, 6),
                                (14, 6),
                                (15, 6),
                                (4, 7),
                                (5, 7),
                                (6, 7),
                                (7, 7),
                                (8, 7),
                                (9, 7),
                                (10, 8),
                                (11, 8),
                                (12, 8),
                                (13, 8),
                                (14, 8),
                                (15, 8)";
            cmd.ExecuteNonQuery();
        }
    }
}
