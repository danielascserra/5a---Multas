using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
       
    {
        //criacao das listas das multas
        public Agentes() {
            ListaDasMultas = new HashSet<Multas>();
    }
        //id, nome, esquadra, foto
        public int ID { get; set; }

        [Required (ErrorMessage ="Por favor, escreva o nome do Agente.")]
        [RegularExpression("([A-ZÁÉÍÓÚÄËÏÖÜa-záéíóúàèùìòõãôâüäöïëçñ]+( |-|')?)+", ErrorMessage = "Só pode escrever letras no nome. Deve começar por uma maiuscula.")]
        public string Nome { get; set; }

        [Required (ErrorMessage ="Não se esqueça de indicar a Esquadra onde o Agente trabalha, por favor.")]
        //[RegularExpression ("(Tomar|Ourém|Torres Novas|Lisboa|Leiria)")]
        public string Esquadra { get; set; }

        public string Fotografia { get; set; }
        
        //a informacao é mais facilmente acedida
        //apresenta a lista das multas passadas por cada agente
        //identifica as multas passadas pelo agente
        //virtual- cria um agente e atribui as multas associadas a ele.
        public virtual ICollection<Multas> ListaDasMultas { get; set; }


        //******************************************************************
        //adicionar uma chave forasteira para a tabela de autenticação
        //[Required]
        public string UserNameId { get; set; }
        public string UsernameId { get; internal set; }
    }
}