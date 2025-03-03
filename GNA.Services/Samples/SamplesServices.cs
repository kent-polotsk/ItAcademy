using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNA.Services.Samples
{
    public interface ITransientService
    {
        public int x { get; }

        public void Do();
    }
    public class TransietnService : ITransientService
    {
        public int x { get; private set; }

        public void Do()
        {
            Console.WriteLine($"Transient {x},");
            x++;
        }
    }

    public interface IScopedService
    {
        public int x { get; }

        public void Do();
    }
    public class ScopedService : IScopedService
    {
        public int x { get; private set; }

        public void Do()
        {
            Console.WriteLine($"Scoped {x},");
            x++;
        }
    }

    public interface ISingletonService
    {
        public int x { get; }

        public void Do();
    }
    public class SingletonService : ISingletonService
    {
        public int x { get; private set; }

        public void Do()
        {
            Console.WriteLine($"Singleton {x},");
            x++;
        }
    }

    public interface ITestService 
    { 
        public void Do();
    }

    public class TestService : ITestService
    {
        private readonly ITransientService _tr1Service;
        private readonly IScopedService _sc1Service;
        private readonly ISingletonService _singlService;

        public TestService(ITransientService tr1Service, 
            IScopedService sc1Service, 
            ISingletonService singlService) 
        {
            _tr1Service = tr1Service;
            _sc1Service = sc1Service;
            _singlService = singlService;
        }

        public void Do()
        {
            _tr1Service.Do();
            _sc1Service.Do();
            _singlService.Do();
        }
    }
}
