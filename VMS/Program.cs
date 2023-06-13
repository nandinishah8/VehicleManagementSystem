namespace VMS
{
   
        public interface IVehicle
        {
            void Drive();
        }
        class Car : IVehicle
        {
            public void Drive()
            {
                Console.WriteLine("driving car");
            }
        }
        class Truck : IVehicle
        {
            public void Drive()
            {
                Console.WriteLine("driving truck");
            }
        }
        //singleton class
        public class VehicleLogger
        {
            private static VehicleLogger? _instance;
            private VehicleLogger() { }
            public static VehicleLogger Instance
            {
                get
                {
                    if (_instance == null) { _instance = new VehicleLogger(); }
                    return _instance;
                }
            }
            public void Log(string message)
            {
                Console.WriteLine("Logging: " + message);
            }
        }
        public abstract class VehicleFactory
        {
            public abstract IVehicle CreateVehicle();
            public void DoSomethingWithVehicle()
            {
                IVehicle vehicle = CreateVehicle();
                vehicle.Drive();
            }
        }
        public class CarFactory : VehicleFactory
        {
            public override IVehicle CreateVehicle()
            {
                return new Car();
            }
        }
        public class TruckFactor : VehicleFactory
        {
            public override IVehicle CreateVehicle()
            {
                return new Truck();
            }
        }
        public interface IRepository<T>
        {
            T GetById(int id);
            IEnumerable<T> GetAll();
            void Add(T entity);
            void Update(T entity);
            void Delete(T entity);
        }
        public class VehicleRepository : IRepository<IVehicle>
        {
            private List<IVehicle> vehicles;
            public VehicleRepository() { vehicles = new List<IVehicle>(); }
            public void Add(IVehicle entity)
            {
                vehicles.Add(entity);
            }
            public void Delete(IVehicle entity)
            {
                vehicles.Remove(entity);
            }
            public IEnumerable<IVehicle> GetAll()
            {
                return vehicles;
            }
            public IVehicle GetById(int id)
            {
                return vehicles[id];
            }
            public void Update(IVehicle entity)
            {
                //uupdate logic
            }
        }
        public class VehicleService
        {
            private readonly IRepository<IVehicle> vehicleRepository;
            public VehicleService(IRepository<IVehicle> repository)
            {
                vehicleRepository = repository;
            }
            public void AddVehicle(VehicleFactory factory)
            {
                IVehicle vehicle = factory.CreateVehicle();
                vehicleRepository.Add(vehicle);
                VehicleLogger.Instance.Log("added vehiclde" + vehicle);
            }
            public void RemoveVehicle(int id)
            {
                IVehicle vehicle = vehicleRepository.GetById(id);
                vehicleRepository.Delete(vehicle);
                VehicleLogger.Instance.Log("removed vehiclde" + vehicle);
            }
            public void ListVehicles()
            {
                IEnumerable<IVehicle> vehicles = vehicleRepository.GetAll();
                foreach (IVehicle vehicle in vehicles)
                {
                    Console.WriteLine(vehicle);
                }
            }
            public void DoSomethingWithVehicle(int id)
            {
            }
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                VehicleFactory carFactory = new CarFactory();
                VehicleFactory truckFactory = new TruckFactor();
                IRepository<IVehicle> vehicleRepositry = new VehicleRepository();
                VehicleService vehicle = new VehicleService(vehicleRepositry);
                vehicle.AddVehicle(carFactory);
                vehicle.AddVehicle(truckFactory);
                vehicle.ListVehicles();
                vehicle.RemoveVehicle(1);
                vehicle.ListVehicles();
            }
        }
    }