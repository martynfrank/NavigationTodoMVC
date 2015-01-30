
namespace NavigationTodoMVC.Tests.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using NavigationTodoMVC.Service;

    [TestClass]
    public class TodoModuleTests
    {
        private TodoModule _todoModule;
        private Mock<ITodoRepository> _todoRepository;

        private List<Todo> _todos;
        private List<Todo> _todosAdded;
        private List<Todo> _todosRemoved;
            
        [TestInitialize]
        public void Setup()
        {
            _todoRepository = new Mock<ITodoRepository>();

            _todos = new List<Todo>();
            _todosAdded = new List<Todo>();
            _todosRemoved = new List<Todo>();

            _todoRepository.Setup(m => m.Todos).Returns(_todos.AsQueryable());
            _todoRepository.Setup(m => m.Add(It.IsAny<Todo>())).Callback<Todo>(c => _todosAdded.Add(c));
            _todoRepository.Setup(m => m.Remove(It.IsAny<Todo>())).Callback<Todo>(c => _todosRemoved.Add(c));

            _todoModule = new TodoModule(_todoRepository.Object);
        }

        #region Get

        [TestMethod]
        public void Get_ManyTodosExistAndAllRequested_AllTodoDetailsReturned()
        {
            // Arrange
            _todos.Add(new Todo { Title = "Todo 1" });
            _todos.Add(new Todo { Title = "Todo 2" });
            _todos.Add(new Todo { Title = "Todo 3" });

            // Act
            var actual = _todoModule.Get(StatusEnum.All);

            // Assert
            Assert.AreEqual(3, actual.Todos.Count());
        }

        [TestMethod]
        public void Get_TodoHasTitle_TodoDetailsReturned()
        {
            // Arrange
            const string title = "Todo 1";
            _todos.Add(new Todo { Title = title });

            // Act
            var actual = _todoModule.Get(StatusEnum.All);

            // Assert
            Assert.AreEqual(1, actual.Todos.Count());
            Assert.AreEqual(title, actual.Todos.First().Title);
        }

        [TestMethod]
        public void Get_OnlyCompletedRequested_CompletedTodosOnlyReturned()
        {
            // Arrange
            _todos.Add(new Todo { Id = 1, Completed = true });
            _todos.Add(new Todo { Id = 2, Completed = false });
            _todos.Add(new Todo { Id = 3, Completed = true });

            // Act
            var actual = _todoModule.Get(StatusEnum.Complete);

            // Assert
            Assert.AreEqual(2, actual.Todos.Count());
            Assert.IsFalse(actual.Todos.Any(x=> x.Id == 2));
        }

        #endregion

        #region Add

        [TestMethod]
        public void Add_NewTodoTitleProvided_TodoAdded()
        {
            // Arrange
            const string newTitle = "I must finish this post";

            // Act
            var actual = _todoModule.Add(newTitle);

            // Assert
            Assert.AreEqual(newTitle, actual.Title);
        }

        #endregion

        #region Remove

        [TestMethod]
        public void Remove_TodoExists_TodoRemoved()
        {
            // Arrange
            const int id = 56;
            _todos.Add(new Todo{Id = id, Title = "Todo Item 1"});

            // Act
            _todoModule.Remove(id);

            // Assert
            Assert.AreEqual(1, _todosRemoved.Count());
            Assert.AreEqual(id, _todosRemoved.First().Id);
        }

        #endregion
    }
}
