using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        //cria var que representa a Base de dados.
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agentes
        public ActionResult Index()
        {
            //procura a totalidade dos agentes na BD
            // Instrução feita em LINQ
            // select * from agentes order by agentes
            var listaAgentes = db.Agentes.OrderBy(a => a.Nome).ToList();
            return View(listaAgentes);
            
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// documentação do código- descrição do metodo (mostra os dados do agente)
        /// </summary>
        /// <param name="id">identifica o Agente</param>
        /// <returns>devolve uma view com os dados</returns>
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");

            }

            //select * from agentes where id=id
            Agentes agente = db.Agentes.Find(id);

            // o Agente foi encontradO?
            if (agente == null)
            {
                // O agente não foi encontrado, porque o utilizador está à pesca
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            //envia para a view os dados do agente que foi procurado e encontrado
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken] - protege contra ataques de outras pessoas por roubo de identidade.

        /// <summary>
        /// criação de um novo agente
        /// </summary>
        /// <param name="agente">recolhe os dados do nome e da esquadra do agente</param>
        /// <param name="fotografia">representa a fotografia que identifica o agente</param>
        /// <returns>devolve uma view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente,
                                    HttpPostedFileBase fotografia)
        {
            //precisamos de processar a fotografia
            //1º sera que forneci um ficheiro?
            //2º será de um tipo correto?
            //3º se for do tipo correto, guarda-se 
            //se nao for, atribui-se um 'avatar generico' ao utilizador

            //var auxiliar
            string caminho = "";
            Boolean haFicheiro = false;

            //há ficheiro?
            if (fotografia == null)
            {
                //nao ha ficheiro
                //atribui-se-lhe o avatar
                agente.Fotografia = "noUser.png";
            }
            else {
                //há ficheiro
                //será correto?
                if (fotografia.ContentType == "image/jpeg" || fotografia.ContentType == "image/png") {
                    //estamos perante uma foto correta
                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
                    //criar um objeto deste tipo
                    Guid g;
                    g = Guid.NewGuid();
                    // nome do ficheiro
                    string nome = g.ToString() + extensao;
                    //onde guardar o ficheiro ve onde esta a pasta principal e avança logo para a pasta imagens, combine junta com a pasta com os ficheiros existentes
                    caminho = Path.Combine(Server.MapPath("~/imagens"), nome);
                    //atribuir ao agente o nome do ficheiro
                    agente.Fotografia = nome;
                    //assinalar que´há foto
                    haFicheiro = true;
                }
            }

            if (ModelState.IsValid) 
            {
                //valida se os dados fornecidos estão de acordo com as regras definidas na especificação do modelo
                //sempre que se usa dados da BD usa-se o try catch
                try
                {
                    //adiciona o novo Agente ao modelo
                    db.Agentes.Add(agente);
                    //consolida os dados na BD
                    db.SaveChanges();
                    //guardar o ficheiro no disco rigido
                    if(haFicheiro)fotografia.SaveAs(caminho);
                    //redireciona o utilizador para a pagina index.
                    return RedirectToAction("Index");
                }
                catch(Exception) {
                    ModelState.AddModelError("", "Ocorreu um erro com a escrita" + "dos dados do novo agente");
                }
            
            }

            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");

            }

            //select * from agentes where id=id
            Agentes agente = db.Agentes.Find(id);

            // o Agente foi encontradO?
            if (agente == null)
            {
                // O agente não foi encontrado, porque o utilizador está à pesca
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agente);

        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");

                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            // o agente foi encontrado
            // vou salvaguardar os dados para posterior validação.
            // - guardar o ID do agente num cookie cifrado (cliente)
            // - guardar o ID numa variavel de sessao (servidor - se se usar o ASP .NET Core, esta ferramenta já nao eviste...)
            // - outras alternativas válidas...

            // 

            Session["Agente"] = agentes.ID;

            //mostra na View os dados do Agente

            return View(agentes);
        }




        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
         
            // procura o agente a remover
            Agentes agente = db.Agentes.Find(id);

            if (agente == null) {
                //nao foi encontrado o agente
                return RedirectToAction("Index");
            }

            // o ID não é null
            // será o ID o que eu espero?
            // vamos validar se o ID está correto
            if (id != (int)Session["Agente"]) {
                //há aqui outro xico esperto..
                return RedirectToAction("Index");
            }

            //procura o agente a remover
            try
            {
           
                // dá ordem de remoção do Agente
            db.Agentes.Remove(agente);
                //consolida a remoção
            db.SaveChanges();

            }
            catch (Exception)
            {
                //devem aqui ser escritas todas as instruções que são consideradas necessárias

                //informar que houve um erro
                ModelState.AddModelError("", "Não é possível remover o Agente." + agente.Nome + "." +
                    "Provavelmente , tem multas associadas a ele...");

                // redirecionar para a página onde o erro foi gerado.
                
                return View(agente);

            }
           

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
