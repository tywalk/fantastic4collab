﻿using fantastic4collab2.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fantastic4collab2.Models
{
    public sealed class Singleton
    {
        private static volatile Singleton instance;
        private static object mutex = new Object();
        private static IDictionary<int, Group> itemCollection;
        private static string connectionString = ConfigurationManager.ConnectionStrings["NotesDB"].ConnectionString;

        private Singleton()
        {
            itemCollection = new Dictionary<int, Group>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string getAllExistingNotesQueryString = "select g.groupid, groupname, itemid, title, content from [Group] g join [Item] i on g.groupid = i.groupid";
                SqlCommand cmd = new SqlCommand(getAllExistingNotesQueryString, sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        int groupID = reader.GetInt32(0);
                        string groupName = reader.GetString(1);
                        int itemID = reader.GetInt32(2);
                        string itemTitle = reader.GetString(3);
                        string itemContent = reader.GetString(4);

                        if (!itemCollection.ContainsKey(groupID))
                        {
                            // Create group
                        }

                        // Add item to group
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public static Singleton Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (mutex)
                    {
                        if(instance == null)
                        {
                            instance = new Singleton();
                        }
                    }
                }

                return instance;

            }
        }

        public Group GetGroup(int groupID)
        {
            if (itemCollection.ContainsKey(groupID))
            {
                return itemCollection[groupID];
            }

            return null;
        }

        public Item GetItem(int groupID, int itemID)
        {
            if (itemCollection.ContainsKey(groupID))
            {
                if (itemCollection[groupID].Contains(itemID))
                {
                    return itemCollection[groupID].GetItem(itemID);
                }
            }

            return null;
        }

        public void AddGroup(Group group)
        {
            itemCollection.Add(group.GroupID, group);
        }

        public void AddItem(int groupID, Item item)
        {
            if (!itemCollection.ContainsKey(groupID))
            {
                // Create new group
            }

            itemCollection[groupID].AddItem(item);
        }
    }
}