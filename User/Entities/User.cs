
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Entities
{
    public class User
    {
        public virtual int Id {get; protected set;}
        
            public virtual string Name {get; set;}

        public User( string Name)
        {
               SetName(Name);
        }

        
            public virtual void SetName(string property)
            {
                  if(property.Length > 200){
                    throw new Exception(Name);
                }

                this.Name = property;
            }
        }
}