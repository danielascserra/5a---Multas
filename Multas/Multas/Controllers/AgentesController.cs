using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        private MultasDB db = new MultasDB();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Agentes.Add(agentes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agentes);
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
