namespace BlogWithAuthentication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using BlogWithAuthentication.Models;
    using Microsoft.Data.Sqlite;
    public class DataAccess : Controller
    {
        public void CreateRecord(PostModel model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.Title = model.Title;
            model.Content = model.Content;
            model.TimeStamp = DateTime.Now.ToString();

            string database = "blog.db";
            string dataSource = $"Data Source={Path.GetFullPath(database)}";
            string query = BuildInsertQuery(model);

            using (SqliteConnection conn = new SqliteConnection(dataSource))
            {
                conn.Open();
                using (SqliteCommand command = new SqliteCommand(query, conn))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        // Return a bool?
                    }
                }
            }
        }

        public void DeleteRecord(PostModel model)
        {
            string database = "blog.db";
            string dataSource = $"Data Source={Path.GetFullPath(database)}";
            string query = BuildDeleteQuery(model);

            using (SqliteConnection conn = new SqliteConnection(dataSource))
            {
                conn.Open();
                using (SqliteCommand command = new SqliteCommand(query, conn))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        // Return a bool?
                    }
                }
            }
        }

        public void UpdateRecord(PostModel model)
        {
            string database = "blog.db";
            string dataSource = $"Data Source={Path.GetFullPath(database)}";
            string query = BuildUpdateQuery(model);

            using (SqliteConnection conn = new SqliteConnection(dataSource))
            {
                conn.Open();
                using (SqliteCommand command = new SqliteCommand(query, conn))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        // Return a bool?
                    }
                }
            }
        }

        private string BuildDeleteQuery(PostModel model)
        {
            return $" DELETE FROM Posts WHERE Id = '{model.Id}' ";
        }

        private string BuildUpdateQuery(PostModel model)
        {
            return $"UPDATE Posts SET Title = '{model.Title}', Content = '{model.Content}' WHERE Id = '{model.Id}'";
        }

        private string BuildInsertQuery(PostModel model)
        {
            return $" INSERT INTO Posts(Id, Title, Content, TimeStamp) " +
                $" VALUES('{model.Id}', '{model.Title}', '{model.Content}', '{model.TimeStamp}') ";
        }

        public List<PostModel> GetPosts()
        {
            string table = "Posts";
            string query = $"select * from {table} limit 25";
            List<PostModel> posts = GetRecords(table, query)
                .OfType<PostModel>().ToList();
            return posts;
        }

        ICollection<object> GetRecords(string table, string query)
        {
            string database = "blog.db";
            string dataSource = $"Data Source={Path.GetFullPath(database)}";
            List<object> records = new List<object>();

            using (SqliteConnection conn = new SqliteConnection(dataSource))
            {
                conn.Open();
                switch (table)
                {
                    case "Posts":
                        using (SqliteCommand command = new SqliteCommand(query, conn))
                        {
                            using (SqliteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    PostModel post = new PostModel()
                                    {
                                        Id = SafeGetString(reader, 0),
                                        Title = SafeGetString(reader, 1),
                                        Content = SafeGetString(reader, 2),
                                        TimeStamp = SafeGetString(reader, 3),
                                    };

                                    records.Add(post);
                                }
                            }
                        }

                        break;
                }
            }

            return records;
        }

        string SafeGetString(SqliteDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}
