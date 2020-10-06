using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace SuperHeroDB.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SuperHeroService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<SuperHero> GetAllHeroes()
        {
            return Data.SuperHeroes;
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetHero/{id}")]

        public SuperHero GetHero(string id)
        {

            return Data.SuperHeroes.Find(sh => sh.Id == int.Parse(id));
        }

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "AddHero", Method = "POST")]

        public SuperHero AddHero(SuperHero hero)
        {
            hero.Id = Data.SuperHeroes.Max(sh => sh.Id) + 1;
            Data.SuperHeroes.Add(hero);
            return hero;
        }


        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateHero/{id}", Method = "PUT")]

        public SuperHero UpdateHero(SuperHero updatedHero, string id)
        {
            SuperHero hero = Data.SuperHeroes.Where(sh => sh.Id == int.Parse(id)).FirstOrDefault();

            hero.FirstName = updatedHero.FirstName;
            hero.LastName = updatedHero.LastName;
            hero.HeroName = updatedHero.HeroName;
            hero.PlaceOfBirth = updatedHero.PlaceOfBirth;
            hero.Combat = updatedHero.Combat;

            return hero;
        }



        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "DeleteHero/{id}", Method = "DELETE")]

        public List<SuperHero> DeleteHero(string id)
        {
            SuperHero hero = Data.SuperHeroes.FirstOrDefault(sh => sh.Id == int.Parse(id));
            Data.SuperHeroes.Remove(hero);
            return Data.SuperHeroes;
        }


        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "SearchHero/{searchTerm}", Method = "GET")]

        public List<SuperHero> SearchHero(string searchTerm)
        {
            List<SuperHero> result = Data.SuperHeroes
                                                              .Where<SuperHero>(sh => sh.FirstName.ToLower().Contains(searchTerm.ToLower())
                                                               || sh.LastName.ToLower().Contains(searchTerm.ToLower())
                                                               || sh.HeroName.ToLower().Contains(searchTerm.ToLower())
                                                                || sh.PlaceOfBirth.ToLower().Contains(searchTerm.ToLower())
                                                              ).ToList<SuperHero>();

            if (result.Count == 0)
            {
                throw new WebFaultException<string>("No super found.", HttpStatusCode.NotFound);
            }


            return result;
        }



        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetSortedHeroList/{type}", Method = "GET")]

        public List<SuperHero> GetSortedHeroList(string type)
        {
            switch (type)
            {
                case "firstname":
                    return Data.SuperHeroes.OrderBy(hero => hero.FirstName).ThenBy(hero => hero.LastName).ToList();
                case "lastname":
                    return Data.SuperHeroes.OrderBy(hero => hero.LastName).ThenBy(hero => hero.FirstName).ToList();
                case "hero":
                    return Data.SuperHeroes.OrderBy(hero => hero.HeroName).ThenBy(hero => hero.LastName).ThenBy(hero => hero.FirstName).ToList();
                case "birthplace":
                    return Data.SuperHeroes.OrderBy(hero => hero.PlaceOfBirth).ThenBy(hero => hero.HeroName).ThenBy(hero => hero.LastName).ThenBy(hero => hero.FirstName).ToList();
                case "combat":
                default:
                    return Data.SuperHeroes.OrderBy(hero => hero.Combat).ThenBy(hero => hero.HeroName).ThenBy(hero => hero.LastName).ThenBy(hero => hero.FirstName).ToList();


            }
        }


        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Fight/{id1}/{id2}", Method = "GET")]

        public string Fight(string id1, string id2)
        {
            SuperHero hero1 = Data.SuperHeroes.Find(hero => hero.Id == int.Parse(id1));
            SuperHero hero2 = Data.SuperHeroes.Find(hero => hero.Id == int.Parse(id2));

            return hero1.Combat > hero2.Combat ? $"{hero1.HeroName} wins!" : hero2.Combat > hero1.Combat ? $"{hero2.HeroName} wins!" : "It is a tie!";
        }

    }


}
