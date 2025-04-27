using DAL.Interfaces;
using DAL.Models;
using EventApi.Interfaces;

namespace EventApi.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MyAppContext _context;
        public OrderService(IUnitOfWork unitOfWork, IMapHelper mapHelper, MyAppContext context)
        {
            _unitOfWork = unitOfWork;

            _context = context;
        }




    }
}
