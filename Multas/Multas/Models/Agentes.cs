using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {
        //id, nome, esquadra, foto
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }
        
        //a informacao é mais facilmente acedida
        //apresenta a lista das multas passadas por cada agente
        //identifica as multas passadas pelo agente
        public ICollection<Multas> ListaDasMultas { get; set; }

    }
}