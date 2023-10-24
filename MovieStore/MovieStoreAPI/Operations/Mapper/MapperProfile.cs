using AutoMapper;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Base.Encryption;

namespace MovieStoreAPI.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile(){
            CreateMap<Movie, MovieViewModel>()
                .ForMember(i => i.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(i => i.Director, opt => opt.MapFrom(src => src.Director.Name + " " + src.Director.Surname))
                .ForMember(i => i.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyy")));
            CreateMap<MovieDTO, Movie>();
            CreateMap<MovieDTO, Movie>().ReverseMap();
            
            
            CreateMap<PaymentDTO, Payment>()
                .ForMember(i => i.PaymentDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Payment, PaymentViewModel>()
                .ForMember(i => i.CustomerFullName, opt => opt.MapFrom(src => src.Customer.Name + " " + src.Customer.Surname))
                .ForMember(i => i.MovieName, opt => opt.MapFrom(src => src.Movie.Name))
                .ForMember(i => i.Price, opt => opt.MapFrom(src => src.Movie.Price))
                .ForMember(i => i.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate));


            CreateMap<Genre, GenreViewModel>();

            
            CreateMap<Director, DirectorViewModel>();
            CreateMap<DirectorDTO, Director>(); 
            CreateMap<DirectorDTO, Director>().ReverseMap();


            CreateMap<Actor, ActorViewModel>();
            CreateMap<ActorDTO, Actor>(); 
            CreateMap<ActorDTO, Actor>().ReverseMap();
            

            CreateMap<Customer, CustomerViewModel>(); 
            CreateMap<CustomerDTO, Customer>()
            .ForMember(i => i.Password , opt => opt.MapFrom(src => Md5.Create(src.Password.ToLower()))); 

        }
    }
}