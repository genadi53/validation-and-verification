using BDJ.Models;
using BDJ.Services;
using MySqlX.XDevAPI.Common;

namespace VV
{
    public class Tests
    {
        private TicketService _ticketService;
        private DiscountCard seniorCard;
        private DiscountCard familyCard;


        [SetUp]
        public void Setup()
        {
            _ticketService = new TicketService();
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

            bool withChild = false;

            DateTime trainLeaveTimeTraficStart1 = new DateTime(2023, 3, 12, 7, 29, 0);
            DateTime trainLeaveTimeTraficStart2 = new DateTime(2023, 3, 12, 7, 30, 0);
            DateTime trainLeaveTimeTraficStart3 = new DateTime(2023, 3, 12, 7, 31, 0);

            var result1 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTimeTraficStart1, withChild, null);
            var result2 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTimeTraficStart2, withChild, null);
            var result3 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTimeTraficStart3, withChild, null);

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

            DateTime trainLeaveTimeTraficEnd1 = new DateTime(2023, 3, 12, 19, 29, 0);
            DateTime trainLeaveTimeTraficEnd2 = new DateTime(2023, 3, 12, 19, 30, 0);
            DateTime trainLeaveTimeTraficEnd3 = new DateTime(2023, 3, 12, 19, 31, 0);


            var result1 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTimeTraficEnd1, withChild, null);
            var result2 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTimeTraficEnd2, withChild, null);
            var result3 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTimeTraficEnd3, withChild, null);

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
        public void caluculateTicketPriceNotyInTrafic()
        {
            double initialPrice = 10.00;
            double expectedPriceNotInTrafic = 9.50;

            bool withChild = false;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 6, 0, 0);
            DateTime trainLeaveTime2 = new DateTime(2023, 3, 12, 12, 30, 0);
            DateTime trainLeaveTime3 = new DateTime(2023, 3, 12, 20, 00, 0);


            var result1 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, null);
            var result2 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, null);
            var result3 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, null);

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

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 9, 30, 0);
            DateTime trainLeaveTime2 = new DateTime(2023, 3, 12, 22, 00, 0);

            var result1 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, seniorCard);
            var result2 = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, seniorCard);

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
            double initialPrice = 10.00;
            double expectedPriceWithCard = 5.00;
            double expectedPriceWithoutCard = 9.00;

            bool withChild = true;

            DateTime trainLeaveTime1 = new DateTime(2023, 3, 12, 9, 30, 0);

            var resultWithCard = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, familyCard);
            var resultWithoutCard = _ticketService.calculateTicketPrice(initialPrice, trainLeaveTime1, withChild, null);

            Console.WriteLine(resultWithCard);
            Console.WriteLine(resultWithoutCard);

            Assert.Multiple(() =>
            {
                Assert.That(resultWithCard, Is.EqualTo(expectedPriceWithCard));
                Assert.That(resultWithoutCard, Is.EqualTo(expectedPriceWithoutCard));
            });
        }
    }
}