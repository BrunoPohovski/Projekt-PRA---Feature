﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataLayer
{
    class FileRepositoryAdmin : IAdminRepository
    {
        private string _path = Settings.Default.ResourceDir + "Administratori.txt";

        public FileRepositoryAdmin() => CreateFileIfNonExistant();


        public void CreateFileIfNonExistant()
        {
            try
            {
                if (!File.Exists(_path))
                {
                    File.Create(_path).Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Administrator deafultAdmin = new Administrator {

                Id = 0,
                Ime="",
                Prezime="",
                Email="admin",
                Lozinka="admin"
            };

            AddAdmin(deafultAdmin);

        }

        public ISet<Administrator> GetAdminsFromFile()
        {
            ISet<Administrator> admini = new HashSet<Administrator>();
            string[] lines = File.ReadAllLines(_path);
            lines.ToList().ForEach(line => admini.Add(Administrator.ParseFromFileLine(line)));
            return admini;
        }

        public void AddAdmin(Administrator admin)
        {
            ISet<Administrator> admini = new HashSet<Administrator>();
            foreach(var a in admini)
            {
                if (a.Equals(admin))
                {
                    Console.WriteLine("File Repo: ADMIN POSTOJI");
                    return;
                }


            }

            admini.Add(admin);
            File.WriteAllLines(_path, admini.Select(a => a.PrepareForFileLine()));
            DataManager.OsobaIdAddOne();
        }

        public Administrator GetAdminByEmailAndPassword(string email, string password) => GetAdminsFromFile().FirstOrDefault(a => a.Email.ToLower() == email.ToLower() && a.Lozinka == password);


        
    }
}
