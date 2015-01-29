
namespace NavigationTodoMVC.Controllers
{
    using System.Web.Mvc;
    using Data;
    using Navigation;
    using NavigationTodoMVC.Models;
    using Service;

    /// <summary>
	/// Todo maintenance Controller.
	/// </summary>
    public class TodoController : Controller
    {
        private readonly TodoModule _todoModule;

	    public TodoController()
	    {
	        _todoModule = new TodoModule(new TodoRepository());
	    }

		/// <summary>
		/// To keep finely-grained Actions in a server-rendered progressively enhanced
		/// SPA the work of building the ViewModel must be delegated to a Child Action.
		/// </summary>
		/// <returns>Index View that executes the _Content Child Action.</returns>
		[ActionSelector]
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// The Child Action that builds the ViewModel for rendering the Todo page.
		/// </summary>
		/// <param name="mode">Todo filter. Can be all, active or completed.</param>
		/// <returns>Todo ViewModel.</returns>
		[ChildActionOnly]
		public ActionResult _Content(string mode)
		{
		    var modeEnum = mode == "complete" ? StatusEnum.Complete : mode == "active" ? StatusEnum.Active : StatusEnum.All;
            var todos = _todoModule.Get(modeEnum);
		    return View(new TodoModel
		    {
		        Todos = todos.Todos,
		        ItemsLeft = todos.ItemsLeft,
		        CompletedCount = todos.CompletedCount
		    });
		}

		/// <summary>
		/// Adds a new todo. Sets the id of this new todo in Context to indicate this
		/// todo's RefreshPanel should be updated.
		/// </summary>
		/// <param name="todoModel">Todo.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Add(TodoModel todoModel)
		{
			if (!string.IsNullOrWhiteSpace(todoModel.NewTitle))
			{
				StateContext.Bag.id = null;
			    var todo =_todoModule.Add(todoModel.NewTitle);
				HttpContext.Items["todoId"] = todo.Id;
			}
			return View();
		}

		/// <summary>
		/// Edits a todo's title. Sets edit to true in Context to indicate the
		/// surrounding RefreshPanels shouldn't be updated.
		/// </summary>
		/// <param name="todo">Todo.</param>
		/// <param name="cancel">Cancel indicator.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Edit(Todo todo, bool cancel = false)
		{
			StateContext.Bag.id = null;
			if (!cancel)
			{
                if (!string.IsNullOrWhiteSpace(todo.Title))
				{
					HttpContext.Items["edit"] = true;
				    _todoModule.Update(todo);
				}
				else
                    _todoModule.Remove(todo.Id);
			} 
			return View();
		}

		/// <summary>
		/// Toggles the todo's Completed status. Sets the todo's id in Context to
		/// indicate this todo's RefreshPanel should be updated.
		/// </summary>
		/// <param name="todo">Todo.</param>
		/// <param name="complete">Status indicator.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Toggle(Todo todo, bool complete)
		{
			HttpContext.Items["todoId"] = StateContext.Bag.id;
			StateContext.Bag.id = null;
            todo.Completed = complete;
            _todoModule.Update(todo);
			return View();
		}

		/// <summary>
		/// Deletes a todo. Sets the todo's id in Context to indicate this todo's
		/// RefreshPanel should be updated.
		/// </summary>
		/// <param name="todo">Todo.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Delete(Todo todo)
		{
			HttpContext.Items["todoId"] = StateContext.Bag.id;
			_todoModule.Remove(todo.Id);
			return View();
		}

		/// <summary>
		/// Toggles all todo's Completed status. Sets refresh to true in Context to
		/// indicate the todo list should be updated.
		/// </summary>
		/// <param name="complete">Complete indicator.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult ToggleAll(bool complete)
		{
			HttpContext.Items["refresh"] = true;
			StateContext.Bag.id = null;
            foreach (var todo in _todoModule.Get(StatusEnum.All).Todos)
            {
                todo.Completed = complete;
                _todoModule.Update(todo);
            }
			return View();
		}

		/// <summary>
		/// Deletes all Completed todos. Sets refresh to true in Context to
		/// indicate the todo list should be updated.
		/// </summary>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult ClearCompleted()
		{
			HttpContext.Items["refresh"] = true;
			StateContext.Bag.id = null;
			_todoModule.RemoveComplete();
			return View();
		}
	}
}