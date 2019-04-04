﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        //id, data, valor, infraccao, FK viatura, FK Agente, FK condutor
        public int ID { get; set; }

        public string Infracao { get; set; }

        public string LocalDaMulta { get; set; }

        public decimal ValorMulta { get; set; }

        public DateTime DataDaMulta { get; set; }

        // Criar chaves forasteiras

        // FK para os agentes
        //usar anotações [HttpPost]
        //o atributo da linha  public int AgenteFK { get; set; } vai ser chave forasteira
        [ForeignKey("Agente")]
        public int AgenteFK { get; set; }
        public Agentes Agente { get; set; }

        //FK para os Condutores
        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; }
        public Condutores Condutor { get; set; }

        //FK para a viatura
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; }
        public Viaturas Viatura { get; set; }


    }
}