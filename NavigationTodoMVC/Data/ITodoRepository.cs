namespace NavigationTodoMVC.Data
{
    using System.Linq;
    using Models;

    public interface ITodoRepository
    {
        /// <summary>
        /// Gets an IQuerable of Todos.
        /// </summary>
        IQueryable<Todo> Todos { get; }

        /// <summary>
        /// Adds a Todo.
        /// </summary>
        /// <param name="todo">The Todo to add.</param>
        void Add(Todo todo);

        /// <summary>
        /// Removes a Todo.
        /// </summary>
        /// <param name="todo">The Todo to remove.</param>
        void Remove(Todo todo);
    }
}
