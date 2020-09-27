using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TicketFlow.Application.Users.Commands;
using TicketFlow.Web.Models.AccountViewModels;

namespace TicketFlow.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn(SignInViewModel model, string returnTo)
        {
            model.Return_To = returnTo;
        
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignIn.Command request, SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Signin");
            }
            else
            {
                request.Email = model.Email;
                request.Password = model.Password;

                await _mediator.Send(request);

                return Redirect(model.Return_To);
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUp.Command request, SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SignUp");
            }
            else
            {
                request = new SignUp.Command
                {
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    Role = "user",
                    CreatedAt = DateTime.Now
                };

                await _mediator.Send(request);

                return RedirectToAction("index", "home");
            }
        }

        [HttpGet]
        public async Task<ActionResult> SignOut(SignOut.Command request, string return_To)
        {
            await _mediator.Send(request);

            return Redirect(return_To);
        }
    }
}
