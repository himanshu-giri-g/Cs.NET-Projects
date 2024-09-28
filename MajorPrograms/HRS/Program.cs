using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservationSystem
{
    public class Room
    {
        public int RoomNumber { get; }
        public string RoomType { get; }
        public double PricePerNight { get; }
        public bool IsAvailable { get; private set; }

        public Room(int roomNumber, string roomType, double pricePerNight)
        {
            RoomNumber = roomNumber;
            RoomType = roomType;
            PricePerNight = pricePerNight;
            IsAvailable = true;
        }

        public void Reserve()
        {
            IsAvailable = false;
        }

        public void Release()
        {
            IsAvailable = true;
        }

        public override string ToString()
        {
            return $"Room {RoomNumber} - {RoomType} (${PricePerNight}/night) - {(IsAvailable ? "Available" : "Reserved")}";
        }
    }

    public class Reservation
    {
        public string GuestName { get; }
        public Room Room { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public Reservation(string guestName, Room room, DateTime startDate, DateTime endDate)
        {
            GuestName = guestName;
            Room = room;
            StartDate = startDate;
            EndDate = endDate;
        }

        public double TotalPrice()
        {
            int numberOfNights = (EndDate - StartDate).Days;
            return Room.PricePerNight * numberOfNights;
        }

        public override string ToString()
        {
            return $"Reservation for {GuestName}: Room {Room.RoomNumber} from {StartDate.ToShortDateString()} to {EndDate.ToShortDateString()} - Total: ${TotalPrice()}";
        }
    }

    public class Customer
    {
        public string Name { get; }
        public string PhoneNumber { get; }
        public string Email { get; }

        public Customer(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public override string ToString()
        {
            return $"{Name}, Phone: {PhoneNumber}, Email: {Email}";
        }
    }

    public class Hotel
    {
        private List<Room> rooms;
        private List<Reservation> reservations;
        private List<Customer> customers;

        public Hotel()
        {
            rooms = new List<Room>();
            reservations = new List<Reservation>();
            customers = new List<Customer>();
        }

        public void AddRoom(int roomNumber, string roomType, double pricePerNight)
        {
            var room = new Room(roomNumber, roomType, pricePerNight);
            rooms.Add(room);
            Console.WriteLine($"Room {roomNumber} added successfully.");
        }

        public void UpdateRoom(int roomNumber, string newRoomType, double newPricePerNight)
        {
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room != null)
            {
                // Here we need to create a new room and remove the old one.
                rooms.Remove(room);
                var updatedRoom = new Room(roomNumber, newRoomType, newPricePerNight);
                rooms.Add(updatedRoom);
                Console.WriteLine($"Room {roomNumber} updated successfully.");
            }
            else
            {
                Console.WriteLine("Room not found.");
            }
        }

        public void DisplayRooms()
        {
            Console.WriteLine("Available Rooms:");
            foreach (var room in rooms)
            {
                Console.WriteLine(room);
            }
        }

        public void MakeReservation(string guestName, int roomNumber, DateTime startDate, DateTime endDate)
        {
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber && r.IsAvailable);
            if (room != null)
            {
                room.Reserve();
                var reservation = new Reservation(guestName, room, startDate, endDate);
                reservations.Add(reservation);
                Console.WriteLine($"Reservation made: {reservation}");
            }
            else
            {
                Console.WriteLine("Room not available for reservation.");
            }
        }

        public void CancelReservation(string guestName, int roomNumber)
        {
            var reservation = reservations.FirstOrDefault(r => r.GuestName.Equals(guestName, StringComparison.OrdinalIgnoreCase) && r.Room.RoomNumber == roomNumber);
            if (reservation != null)
            {
                reservation.Room.Release();
                reservations.Remove(reservation);
                Console.WriteLine($"Reservation for {guestName} in room {roomNumber} canceled successfully.");
            }
            else
            {
                Console.WriteLine("Reservation not found.");
            }
        }

        public void ModifyReservation(string guestName, int roomNumber, DateTime newStartDate, DateTime newEndDate)
        {
            var reservation = reservations.FirstOrDefault(r => r.GuestName.Equals(guestName, StringComparison.OrdinalIgnoreCase) && r.Room.RoomNumber == roomNumber);
            if (reservation != null)
            {
                // Cancel the existing reservation
                CancelReservation(guestName, roomNumber);
                // Create a new reservation
                MakeReservation(guestName, roomNumber, newStartDate, newEndDate);
            }
            else
            {
                Console.WriteLine("Reservation not found.");
            }
        }

        public void DisplayReservations()
        {
            Console.WriteLine("Current Reservations:");
            foreach (var reservation in reservations)
            {
                Console.WriteLine(reservation);
            }
        }

        public void AddCustomer(string name, string phoneNumber, string email)
        {
            var customer = new Customer(name, phoneNumber, email);
            customers.Add(customer);
            Console.WriteLine($"Customer added: {customer}");
        }

        public void SearchAvailableRooms(DateTime startDate, DateTime endDate)
        {
            var reservedRooms = reservations
                .Where(r => (r.StartDate < endDate && r.EndDate > startDate))
                .Select(r => r.Room.RoomNumber)
                .ToHashSet();

            var availableRooms = rooms.Where(r => !reservedRooms.Contains(r.RoomNumber)).ToList();

            Console.WriteLine("Available Rooms for the selected dates:");
            foreach (var room in availableRooms)
            {
                Console.WriteLine(room);
            }
        }

        public void GenerateReport()
        {
            Console.WriteLine("Hotel Report:");
            Console.WriteLine($"Total Rooms: {rooms.Count}");
            Console.WriteLine($"Total Reservations: {reservations.Count}");
            Console.WriteLine($"Total Customers: {customers.Count}");
            Console.WriteLine("Current Reservations:");
            foreach (var reservation in reservations)
            {
                Console.WriteLine(reservation);
            }
        }

        public void DisplayCustomerDetails(string customerName)
        {
            var customer = customers.FirstOrDefault(c => c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));
            if (customer != null)
            {
                Console.WriteLine($"Customer Details: {customer}");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public void DisplayRoomDetails(int roomNumber)
        {
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room != null)
            {
                Console.WriteLine(room);
            }
            else
            {
                Console.WriteLine("Room not found.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Hotel hotel = new Hotel();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nHotel Reservation System");
                Console.WriteLine("1. Add Room");
                Console.WriteLine("2. Update Room");
                Console.WriteLine("3. Display Rooms");
                Console.WriteLine("4. Make Reservation");
                Console.WriteLine("5. Cancel Reservation");
                Console.WriteLine("6. Modify Reservation");
                Console.WriteLine("7. Display Reservations");
                Console.WriteLine("8. Add Customer");
                Console.WriteLine("9. Search Available Rooms");
                Console.WriteLine("10. Generate Report");
                Console.WriteLine("11. Display Customer Details");
                Console.WriteLine("12. Display Room Details");
                Console.WriteLine("13. Exit");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter room number: ");
                        int roomNumber = int.Parse(Console.ReadLine());
                        Console.Write("Enter room type: ");
                        string roomType = Console.ReadLine();
                        Console.Write("Enter price per night: ");
                        double pricePerNight = double.Parse(Console.ReadLine());
                        hotel.AddRoom(roomNumber, roomType, pricePerNight);
                        break;

                    case "2":
                        Console.Write("Enter room number to update: ");
                        int updateRoomNumber = int.Parse(Console.ReadLine());
                        Console.Write("Enter new room type: ");
                        string newRoomType = Console.ReadLine();
                        Console.Write("Enter new price per night: ");
                        double newPricePerNight = double.Parse(Console.ReadLine());
                        hotel.UpdateRoom(updateRoomNumber, newRoomType, newPricePerNight);
                        break;

                    case "3":
                        hotel.DisplayRooms();
                        break;

                    case "4":
                        Console.Write("Enter guest name: ");
                        string guestName = Console.ReadLine();
                        Console.Write("Enter room number: ");
                        int reserveRoomNumber = int.Parse(Console.ReadLine());
                        Console.Write("Enter start date (yyyy-mm-dd): ");
                        DateTime startDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter end date (yyyy-mm-dd): ");
                        DateTime endDate = DateTime.Parse(Console.ReadLine());
                        hotel.MakeReservation(guestName, reserveRoomNumber, startDate, endDate);
                        break;

                    case "5":
                        Console.Write("Enter guest name: ");
                        string cancelGuestName = Console.ReadLine();
                        Console.Write("Enter room number: ");
                        int cancelRoomNumber = int.Parse(Console.ReadLine());
                        hotel.CancelReservation(cancelGuestName, cancelRoomNumber);
                        break;

                    case "6":
                        Console.Write("Enter guest name: ");
                        string modifyGuestName = Console.ReadLine();
                        Console.Write("Enter room number: ");
                        int modifyRoomNumber = int.Parse(Console.ReadLine());
                        Console.Write("Enter new start date (yyyy-mm-dd): ");
                        DateTime newStartDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter new end date (yyyy-mm-dd): ");
                        DateTime newEndDate = DateTime.Parse(Console.ReadLine());
                        hotel.ModifyReservation(modifyGuestName, modifyRoomNumber, newStartDate, newEndDate);
                        break;

                    case "7":
                        hotel.DisplayReservations();
                        break;

                    case "8":
                        Console.Write("Enter customer name: ");
                        string customerName = Console.ReadLine();
                        Console.Write("Enter phone number: ");
                        string phoneNumber = Console.ReadLine();
                        Console.Write("Enter email: ");
                        string email = Console.ReadLine();
                        hotel.AddCustomer(customerName, phoneNumber, email);
                        break;

                    case "9":
                        Console.Write("Enter start date (yyyy-mm-dd): ");
                        DateTime searchStartDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter end date (yyyy-mm-dd): ");
                        DateTime searchEndDate = DateTime.Parse(Console.ReadLine());
                        hotel.SearchAvailableRooms(searchStartDate, searchEndDate);
                        break;

                    case "10":
                        hotel.GenerateReport();
                        break;

                    case "11":
                        Console.Write("Enter customer name to view details: ");
                        string detailCustomerName = Console.ReadLine();
                        hotel.DisplayCustomerDetails(detailCustomerName);
                        break;

                    case "12":
                        Console.Write("Enter room number to view details: ");
                        int detailRoomNumber = int.Parse(Console.ReadLine());
                        hotel.DisplayRoomDetails(detailRoomNumber);
                        break;

                    case "13":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
    }
}
