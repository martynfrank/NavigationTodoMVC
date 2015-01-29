
namespace NavigationTodoMVC.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Models;

    public class TodoRepository
    {
        private readonly HttpContext _context = HttpContext.Current;

        private int _id
        {
            get { return (int)(_context.Session["IdCount"] ?? 1); }
            set { _context.Session["IdCount"] = value; }
        }

        private List<Todo> todos
        {
            get
            {
                if (_context.Session["todos"] == null)
                    _context.Session["todos"] = new List<Todo>();
                return (List<Todo>)_context.Session["todos"];
            }
        }

        /// <summary>
        /// Gets an IQuerable of Todos.
        /// </summary>
        public IQueryable<Todo> Todos
        {
            get
            {
                return todos.AsQueryable();
            }
        }

        /// <summary>
        /// Adds a Todo.
        /// </summary>
        /// <param name="todo">The Todo to add.</param>
        public void Add(Todo todo)
        {
            todo.Id = _id++;
            todos.Add(todo);
        }

        /// <summary>
        /// Removes a Todo.
        /// </summary>
        /// <param name="todo">The Todo to remove.</param>
        public void Remove(Todo todo)
        {
            todos.Remove(todo);
        }
    }
}