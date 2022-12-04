using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TheSimchaFundData
{
    public class TheSimchaFundManager
    {
        private string _connectionString;

        public TheSimchaFundManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public List<Person> GetPeople()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM People";
            var result = new List<Person>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Person
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Balance = GetBalance((int)reader["Id"]),
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    Date = (DateTime)reader["Date"],
                    Include = (bool)reader["Include"]
                });


            }
            return result;
        }

        public Person GetPerson(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM People WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            var result = new List<Person>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read() == false)
            {
                return null;
            }
            else 
            {
                return (new Person
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Balance = GetBalance((int)reader["Id"]),
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    Date = (DateTime)reader["Date"]
                });
            } 

        }


        public List<Simcha> GetSimchas()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Simchas";
            var result = new List<Simcha>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Total = GetTotalForSimcha((int)reader["Id"]),
                    Date = (DateTime)reader["Date"],
                    ContributorCount = GetContributorsForSimcha((int)reader["Id"])
                });


            }
            return result;
        }

        public Simcha GetSimcha(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Simchas WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            var result = new List<Simcha>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            if(reader.Read() == false)
            {
                return null;
            }
            else
            {
                return (new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Total = GetTotalForSimcha((int)reader["Id"]),
                    Date = (DateTime)reader["Date"],
                    ContributorCount = GetContributorsForSimcha((int)reader["Id"])
                });

            }
        }
        public decimal GetBalance(int id)
        {  
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Deposits WHERE PersonId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            decimal result = 0;
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result += (decimal)reader["Amount"];
            }
            connection.Close();
            connection.Dispose();
            var conn = new SqlConnection(_connectionString);
            var command = conn.CreateCommand();
            command.CommandText = @"SELECT * FROM Contributions WHERE PersonId = @id";
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            var rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                result -= (decimal)rdr["Amount"];
            }
            return result;
        }

        public decimal GetTotalForSimcha(int simchaId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributions WHERE SimchaId = @id";
            cmd.Parameters.AddWithValue("@id", simchaId);
            decimal result = 0;
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result += (decimal)reader["Amount"];
            }
            return result;
        }

        public int GetContributorsForSimcha(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributions WHERE SimchaId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            int result = 0;
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result++;
            }
            return result;
        }
        public int AddSimcha(Simcha simcha)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Simchas(Name, Date) VALUES(@name, @date) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", simcha.Name);
            cmd.Parameters.AddWithValue("@date", simcha.Date);
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
            
        }
        public decimal GetTotalBalances()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Deposits";
            decimal result = 0;
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result += (decimal)reader["Amount"];
            }
            connection.Close();
            connection.Dispose();
            var conn = new SqlConnection(_connectionString);
            var command = conn.CreateCommand();
            command.CommandText = @"SELECT * FROM Contributions";
            conn.Open();
            var rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                result -= (decimal)rdr["Amount"];
            }
            return result;
        }
        public int AddPerson(Person person)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO People(FirstName, LastName, PhoneNumber, AlwaysInclude, Date, Include) VALUES(@firstName, @lastName, @phoneNumber, 
                    @alwaysInclude, @date, @include) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@firstName", person.FirstName);
            cmd.Parameters.AddWithValue("@lastName", person.LastName);
            cmd.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
            cmd.Parameters.AddWithValue("@alwaysInclude", person.AlwaysInclude);
            cmd.Parameters.AddWithValue("@date", person.Date);
            cmd.Parameters.AddWithValue("@include", false);
            connection.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();
            AddDeposit(new Deposit
            {
                    
                Amount = person.Amount,
                PersonId = id,
                Date = person.Date
            }) ;

            return id;
        }
        public void EditPerson(Person person)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE People SET FirstName = @firstName, LastName = @lastName, PhoneNumber = @phoneNumber, 
                            AlwaysInclude = @alwaysInclude, Date = @date WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", person.Id);
            cmd.Parameters.AddWithValue("@firstName", person.FirstName);
            cmd.Parameters.AddWithValue("@lastName", person.LastName);
            cmd.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
            cmd.Parameters.AddWithValue("@alwaysInclude", person.AlwaysInclude);
            cmd.Parameters.AddWithValue("@date", person.Date);
            cmd.Parameters.AddWithValue("@include", false);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void AddDeposit(Deposit deposit)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits(Amount, PersonId, Date) VALUES(@amount, @personId, @date)";
            cmd.Parameters.AddWithValue("@amount", deposit.Amount);
            cmd.Parameters.AddWithValue("@personId", deposit.PersonId);
            cmd.Parameters.AddWithValue("@date", deposit.Date);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

        }

        public void AddContributions(List<Contribution> contributions)
        {
            foreach(Contribution c in contributions)
            {
                AddContribution(c);
            }
        }

        public void AddContribution(Contribution contribution)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributions(Amount, PersonId, SimchaId, Date) VALUES(@amount, @personId, @simchaId, @date)";
            cmd.Parameters.AddWithValue("@amount", contribution.Amount);
            cmd.Parameters.AddWithValue("@personId", contribution.PersonId);
            cmd.Parameters.AddWithValue("@simchaId", contribution.SimchaId);
            cmd.Parameters.AddWithValue("@date", contribution.Date);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

        }

        public bool IdsExistInContributions(int personId, int simchaId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributions WHERE PersonId = @personId AND SimchaId = @simchaId";
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if(reader.Read() == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdateContribution(Contribution contribution)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Contributions SET Amount = @amount, Date = @date WHERE PersonId = @personId AND SimchaId = @simchaId";
            cmd.Parameters.AddWithValue("@amount", contribution.Amount);
            cmd.Parameters.AddWithValue("@personId", contribution.PersonId);
            cmd.Parameters.AddWithValue("@simchaId", contribution.SimchaId);
            cmd.Parameters.AddWithValue("@date", contribution.Date);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateInclude(Person person)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE People SET Include = @include WHERE Id = @id";
            cmd.Parameters.AddWithValue("@include", person.Include);
            cmd.Parameters.AddWithValue("@id", person.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteContribution(Contribution contribution)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Contributions WHERE PersonId = @personId AND SimchaId = @simchaId";
            cmd.Parameters.AddWithValue("@amount", contribution.Amount);
            cmd.Parameters.AddWithValue("@personId", contribution.PersonId);
            cmd.Parameters.AddWithValue("@simchaId", contribution.SimchaId);
            cmd.Parameters.AddWithValue("@date", contribution.Date);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public List<MoneyAction> GetHistory(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT c.Amount, c.Date, s.Name
                        FROM Contributions c
                        JOIN Simchas s
                        ON s.Id = c.SimchaId
                        WHERE c.PersonId = @id
                        GROUP BY c.Amount, c.Date, s.Name";
            cmd.Parameters.AddWithValue("@id", id);
            List<MoneyAction> result = new List<MoneyAction>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new MoneyAction
                {
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],
                    SimchaName = (string)reader["Name"]
                });
            }
            connection.Close();
            connection.Dispose();
            var conn = new SqlConnection(_connectionString);
            var command = conn.CreateCommand();
            command.CommandText = @"SELECT Amount, Date FROM Deposits WHERE PersonId = @id";
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            var rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                result.Add(new MoneyAction
                {
                    Amount = (decimal)rdr["Amount"],
                    Date = (DateTime)rdr["Date"]
                });
            }
            return result;
        }
        public List<Contribution> GetContributionsForSimcha(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT Amount, Date, SimchaId, PersonId FROM Contributions WHERE SimchaId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            List<Contribution> result = new List<Contribution>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Contribution
                {
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],
                    SimchaId = (int)reader["SimchaId"],
                    PersonId = (int)reader["PersonId"]
                });
            }
            connection.Close();
            return result;
        }

        public void AddContributionsForSimcha(List<Person> people, int simchaId)
        {
           foreach(Person p in people)
            {
                UpdateInclude(p);
                if(p.Include == true)
                {
                    if(IdsExistInContributions(p.Id, simchaId))
                    {
                        UpdateContribution(new Contribution
                        {
                            Amount = p.Amount,
                            PersonId = p.Id,
                            SimchaId = simchaId,
                            Date = DateTime.Now
                        });

                    }
                    else
                    {
                        AddContribution(new Contribution
                        {
                            Amount = p.Amount,
                            PersonId = p.Id,
                            SimchaId = simchaId,
                            Date = DateTime.Now
                        });
                    }
                }
                else
                {
                    DeleteContribution(new Contribution
                    {
                        Amount = p.Amount,
                        PersonId = p.Id,
                        SimchaId = simchaId,
                        Date = DateTime.Now
                    });
                }
            }
        }
    }
    public class Simcha
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public int ContributorCount { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public bool AlwaysInclude { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool Include { get; set; }
    }

    public class MoneyAction
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string SimchaName { get; set; }
    }

    public class Deposit
    {
        public decimal Amount { get; set;}
        public int PersonId { get; set; }
        public DateTime Date { get; set; }
    }

    public class Contribution
    {
        public decimal Amount { get; set; }
        public int PersonId { get; set; }
        public int SimchaId { get; set; }
        public DateTime Date { get; set; }

    }
}
