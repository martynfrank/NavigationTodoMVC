
namespace NavigationTodoMVC.Service
{
    using System.Linq;
    using Data;
    using Models;

    public class TodoModule
    {
        private readonly ITodoRepository _todoRepository;
        public TodoModule(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        /// <summary>
        /// Gets a list of Todos. Either All, Active or Complete can be returned.
        /// </summary>
        /// <param name="status">The status to be returned.</param>
        /// <returns>The TodoDto containing items and counts.</returns>
        public TodoDto Get(StatusEnum status)
        {
            var todos = _todoRepository.Todos.ToList();
            var todoDto = new TodoDto
            {
                Todos = todos,
                ItemsLeft = todos.Count(t => !t.Completed),
                CompletedCount = todos.Count(t => t.Completed)
            };
            if (status != StatusEnum.All)
                todoDto.Todos = todos.Where(t => t.Completed == (status == StatusEnum.Complete));
            return todoDto;
        }

        /// <summary>
        /// Adds a new todo.
        /// </summary>
        /// <param name="title">The title of the todo.</param>
        /// <returns>The newly added Todo.</returns>
        public Todo Add(string title)
        {
            var todo = new Todo {Title = title};
            _todoRepository.Add(todo);
            return todo;
        }

        /// <summary>
        /// Removed a todo based on the id.
        /// </summary>
        /// <param name="id">The Id to remove.</param>
        public void Remove(int id)
        {
            var todo = _todoRepository.Todos.First(t => t.Id == id);
            _todoRepository.Remove(todo);
        }

        /// <summary>
        /// Updates a Todo details (completed, title), if the Title is null it will not be updated.
        /// </summary>
        /// <param name="todo">The todo detaisl to update.</param>
        public void Update(Todo todo)
        {
            var todoDetails = _todoRepository.Todos.First(t => t.Id == todo.Id);
            if (!string.IsNullOrEmpty(todo.Title))
                todoDetails.Title = todo.Title;
            todoDetails.Completed = todo.Completed;
        }

        /// <summary>
        /// Removes all completed todos.
        /// </summary>
        public void RemoveComplete()
        {
            var completed = _todoRepository.Todos.Where(t => t.Completed);
            foreach (var complete in completed)
            {
                _todoRepository.Remove(complete);
            }
        }
    }
}