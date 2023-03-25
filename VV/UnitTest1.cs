using BDJ;
using BDJ.Models;
using BDJ.Services;
using MySqlX.XDevAPI.Common;

namespace VV
{
    public class Tests
    {
        
        private TrainSystemContext _trainSystemContext;
        private TicketService _ticketService;
        private DiscountCard seniorCard;
        private DiscountCard familyCard;


        [SetUp]
        public void Setup()
        {
            _trainSystemContext = new TrainSystemContext();
            _ticketService = new TicketService(_trainSystemContext);
            seniorCard = new DiscountCard { Id = 1, Type = "senior", UserId = 1 };
            familyCard = new DiscountCard { Id = 2, Type = "family", UserId = 2 };

        }

        [Test]
        [Category("Trafic")]
        public void caluculateTicketPriceInEarlyTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceInTrafic = 10.00;
            double expectedPriceNotInTrafic = 9.50;

            int numberOfTickets = 1;
            bool withChild = false;

            DateTime trainLeaveTimeTraficStart1 = new DateTime(2023, 3, 12, 7, 29, 0);
            DateTime trainLeaveTimeTraficStart2 = new DateTime(2023, 3, 12, 7, 30, 0);
            DateTime trainLeaveTimeTraficStart3 = new DateTime(2023, 3, 12, 7, 31, 0);

            var result1 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTimeTraficStart1, withChild, null);
            var result2 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTimeTraficStart2, withChild, null);
            var result3 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTimeTraficStart3, withChild, null);

            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPriceNotInTrafic));
                Assert.That(result2, Is.EqualTo(expectedPriceInTrafic));
                Assert.That(result3, Is.EqualTo(expectedPriceInTrafic));
            });
        }

        [Test]
        [Category("Trafic")]
        public void caluculateTicketPriceInLateTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceInTrafic = 10.00;
            double expectedPriceNotInTrafic = 9.50;

            bool withChild = false;
            int numberOfTickets = 1;

            DateTime trainLeaveTimeTraficEnd1 = new DateTime(2023, 3, 12, 19, 29, 0);
            DateTime trainLeaveTimeTraficEnd2 = new DateTime(2023, 3, 12, 19, 30, 0);
            DateTime trainLeaveTimeTraficEnd3 = new DateTime(2023, 3, 12, 19, 31, 0);


            var result1 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTimeTraficEnd1, withChild, null);
            var result2 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTimeTraficEnd2, withChild, null);
            var result3 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTimeTraficEnd3, withChild, null);

            //Console.WriteLine(result1);
            //Console.WriteLine(result2);
            //Console.WriteLine(result3);


            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPriceInTrafic));
                Assert.That(result2, Is.EqualTo(expectedPriceInTrafic));
                Assert.That(result3, Is.EqualTo(expectedPriceNotInTrafic));
            });
        }

        [Test]
        [Category("Trafic")]
        public void caluculateTicketPriceNotInTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceNotInTrafic = 9.50;

            bool withChild = false;
            int numberOfTickets = 1;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 6, 0, 0);
            DateTime trainLeaveTime2 = new DateTime(2023, 3, 12, 12, 30, 0);
            DateTime trainLeaveTime3 = new DateTime(2023, 3, 12, 20, 00, 0);


            var result1 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime1, withChild, null);
            var result2 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime2, withChild, null);
            var result3 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime3, withChild, null);

            //Console.WriteLine(result1);
            //Console.WriteLine(result2);
            //Console.WriteLine(result3);


            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPriceNotInTrafic));
                Assert.That(result2, Is.EqualTo(expectedPriceNotInTrafic));
                Assert.That(result3, Is.EqualTo(expectedPriceNotInTrafic));
            });
        }

        [Test]
        [Category("With Senior Card")]
        public void caluculateTicketPriceWithSeniorDiscount()
        {
            double initialPrice = 10.00;
            double expectedPrice = 10.00 * 0.66;

            bool withChild = false;
            int numberOfTickets = 1;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 9, 30, 0);
            DateTime trainLeaveTime2 = new DateTime(2023, 3, 12, 22, 00, 0);

            var result1 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime1, withChild, seniorCard);
            var result2 = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime1, withChild, seniorCard);

            Console.WriteLine(result1);
            Console.WriteLine(result2);

            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPrice));
                Assert.That(result2, Is.EqualTo(expectedPrice));
            });
        }

        [Test]
        [Category("With Family Card")]
        public void CalculateTicketPriceWithFamilyDiscount()
        {
            bool withChild = true;
            int numberOfTickets = 2;

            double initialPrice = 10.00;
            double expectedPriceWithoutCard = 9.00 * numberOfTickets;
            double expectedPriceWithCard = 5.00 * numberOfTickets;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 9, 30, 0);

            var resultWithCard = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime1, withChild, familyCard);
            var resultWithoutCard = _ticketService.CalculateTicketPrice(initialPrice, numberOfTickets, trainLeaveTime1, withChild, null);

            Console.WriteLine(resultWithCard);
            Console.WriteLine(resultWithoutCard);

            Assert.Multiple(() =>
            {
                Assert.That(resultWithCard, Is.EqualTo(expectedPriceWithCard));
                Assert.That(resultWithoutCard, Is.EqualTo(expectedPriceWithoutCard));
            });
        }

        [Test]
        [Category("Invalid data")]
        public void caluculateTicketPriceInvalid()
        {
            double expectedReturn = -1;
            
            double invalidInitialPrice = 0;
            double initialPrice = 10;
            int numberOfTickets = 1;
            int invalidNumberOfTickets = 0;

            bool withChild = false;

            DateTime trainLeaveTime = new DateTime(2023, 3, 19, 7, 29, 0);

            var result1 = _ticketService.CalculateTicketPrice(invalidInitialPrice, numberOfTickets, trainLeaveTime, withChild, null);
            var result2= _ticketService.CalculateTicketPrice(initialPrice, invalidNumberOfTickets, trainLeaveTime, withChild, null);

           
            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedReturn));
                Assert.That(result2, Is.EqualTo(expectedReturn));
            });
        }

        [Test]
        [Category("Two-way Trafic")]
        public void caluculateTwoWayTicketPriceInEarlyTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceInTrafic = 10.00 * 2;
            double expectedPriceNotInTrafic = 9.50 * 2;

            int numberOfTickets = 1;
            bool withChild = false;
            bool twoWay = true;

            DateTime trainLeaveTimeTraficStart1 = new DateTime(2023, 3, 12, 7, 29, 0);
            DateTime trainLeaveTimeTraficStart2 = new DateTime(2023, 3, 12, 7, 30, 0);
            DateTime trainLeaveTimeTraficStart3 = new DateTime(2023, 3, 12, 7, 31, 0);

            var result1 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTimeTraficStart1, withChild, null);
            var result2 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTimeTraficStart2, withChild, null);
            var result3 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTimeTraficStart3, withChild, null);

            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPriceNotInTrafic));
                Assert.That(result2, Is.EqualTo(expectedPriceInTrafic));
                Assert.That(result3, Is.EqualTo(expectedPriceInTrafic));
            });
        }

        [Test]
        [Category("Two-way Trafic")]
        public void caluculateTwoWayTicketPriceInLateTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceInTrafic = 10.00 * 2;
            double expectedPriceNotInTrafic = 9.50 * 2;

            bool withChild = false;
            int numberOfTickets = 1;
            bool twoWay = true;

            DateTime trainLeaveTimeTraficEnd1 = new DateTime(2023, 3, 12, 19, 29, 0);
            DateTime trainLeaveTimeTraficEnd2 = new DateTime(2023, 3, 12, 19, 30, 0);
            DateTime trainLeaveTimeTraficEnd3 = new DateTime(2023, 3, 12, 19, 31, 0);


            var result1 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTimeTraficEnd1, withChild, null);
            var result2 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTimeTraficEnd2, withChild, null);
            var result3 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTimeTraficEnd3, withChild, null);

            //Console.WriteLine(result1);
            //Console.WriteLine(result2);
            //Console.WriteLine(result3);


            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPriceInTrafic));
                Assert.That(result2, Is.EqualTo(expectedPriceInTrafic));
                Assert.That(result3, Is.EqualTo(expectedPriceNotInTrafic));
            });
        }

        [Test]
        [Category("Two-way Trafic")]
        public void caluculateTwoWayTicketPriceNotInTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceNotInTrafic = 9.50 * 2;

            bool withChild = false;
            int numberOfTickets = 1;
            bool twoWay = true;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 6, 0, 0);
            DateTime trainLeaveTime2 = new DateTime(2023, 3, 12, 12, 30, 0);
            DateTime trainLeaveTime3 = new DateTime(2023, 3, 12, 20, 00, 0);


            var result1 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime1, withChild, null);
            var result2 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime2, withChild, null);
            var result3 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime3, withChild, null);

            Console.WriteLine(result1);
            Console.WriteLine(result2);
            Console.WriteLine(result3);


            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPriceNotInTrafic));
                Assert.That(result2, Is.EqualTo(expectedPriceNotInTrafic));
                Assert.That(result3, Is.EqualTo(expectedPriceNotInTrafic));
            });
        }

        [Test]
        [Category("Two-way With Senior Card")]
        public void caluculateTwoWayTicketPriceWithSeniorDiscount()
        {
            double initialPrice = 10.00;
            double expectedPrice = 10.00 * 0.66 * 2;

            bool withChild = false;
            int numberOfTickets = 1;
            bool twoWay = true;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 9, 30, 0);
            DateTime trainLeaveTime2 = new DateTime(2023, 3, 12, 22, 00, 0);

            var result1 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime1, withChild, seniorCard);
            var result2 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime1, withChild, seniorCard);

            Console.WriteLine(result1);
            Console.WriteLine(result2);

            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedPrice));
                Assert.That(result2, Is.EqualTo(expectedPrice));
            });
        }

        [Test]
        [Category("Two-way With Family Card")]
        public void CalculateTwoWayTicketPriceWithFamilyDiscount()
        {
            bool withChild = true;
            int numberOfTickets = 2;
            bool twoWay = true;

            double initialPrice = 10.00;
            double expectedPriceWithoutCard = 9.00 * numberOfTickets *2;
            double expectedPriceWithCard = 5.00 * numberOfTickets * 2;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 9, 30, 0);

            var resultWithCard = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime1, withChild, familyCard);
            var resultWithoutCard = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, numberOfTickets, trainLeaveTime1, withChild, null);

            Console.WriteLine(resultWithCard);
            Console.WriteLine(resultWithoutCard);

            Assert.Multiple(() =>
            {
                Assert.That(resultWithCard, Is.EqualTo(expectedPriceWithCard));
                Assert.That(resultWithoutCard, Is.EqualTo(expectedPriceWithoutCard));
            });
        }

        [Test]
        [Category("Two-way Invalid data")]
        public void caluculateTwoWayTicketPriceInvalid()
        {
            double expectedReturn = -1;

            double invalidInitialPrice = 0;
            double initialPrice = 10;
            int numberOfTickets = 1;
            int invalidNumberOfTickets = 0;

            bool withChild = false;
            bool twoWay = true;

            DateTime trainLeaveTime = new DateTime(2023, 3, 19, 7, 29, 0);

            var result1 = _ticketService.CalculateTwoWayTicketPrice(twoWay, invalidInitialPrice, numberOfTickets, trainLeaveTime, withChild, null);
            var result2 = _ticketService.CalculateTwoWayTicketPrice(twoWay, initialPrice, invalidNumberOfTickets, trainLeaveTime, withChild, null);

            Console.WriteLine(result1);
            Console.WriteLine(result2);

            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo(expectedReturn));
                Assert.That(result2, Is.EqualTo(expectedReturn));
            });
        }
    }
}