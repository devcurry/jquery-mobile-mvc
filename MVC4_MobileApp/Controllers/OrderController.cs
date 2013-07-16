using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MVC4_MobileApp.Models;

namespace MVC4_MobileApp.Controllers
{
    public class OrderController : ApiController
    {
        private jQMOrderDBEntities db = new jQMOrderDBEntities();

        // GET api/Order
        public IEnumerable<Order> GetOrders()
        {
            var orders = db.Orders.Include("Product");
            return orders.AsEnumerable();
        }

        // GET api/Order/5
        public Order GetOrder(int id)
        {
            Order order = db.Orders.Single(o => o.OrderId == id);
            if (order == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return order;
        }

        // PUT api/Order/5
        public HttpResponseMessage PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != order.OrderId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Orders.Attach(order);
            db.ObjectStateManager.ChangeObjectState(order, EntityState.Modified);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Order
        public HttpResponseMessage PostOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.AddObject(order);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, order);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = order.OrderId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Order/5
        public HttpResponseMessage DeleteOrder(int id)
        {
            Order order = db.Orders.Single(o => o.OrderId == id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Orders.DeleteObject(order);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}