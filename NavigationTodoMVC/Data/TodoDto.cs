
namespace NavigationTodoMVC.Data
{
    using System.Collections.Generic;
    using Models;

    public class TodoDto
    {
        public IEnumerable<Todo> Todos { get; set; }
        public int ItemsLeft { get; set; }
        public int CompletedCount { get; set; }
    }
}