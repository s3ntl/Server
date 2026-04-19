using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTools.Database
{
    public interface IDatabase
    {
        string ConnectionString { get; set; }

        void Init(string PathToDatabase);

        
    }
}
