using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Enemy
    {


        public int Id { get; set; }

        public string Name { get; set; }

        public int Health { get; set; }

        public string ImagePath { get; set; }

        public Enemy()
        {

        }

    }
}
