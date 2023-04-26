using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ekomplet.services;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace ekomplet.Webapp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IInstallerRepository installerContext;
        private readonly ISupervisorRepository supervisorContext;
        private readonly ILogger<EmployeesController> logger;

        public EmployeesController(IInstallerRepository installerContext, ISupervisorRepository supervisorContext, ILogger<EmployeesController> logger)
        {
            this.installerContext = installerContext;
            this.supervisorContext = supervisorContext;
            this.logger = logger;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                var installers = await installerContext.GetAllInstallers();
                var supervisors = await supervisorContext.GetAllSupervisor();
                List<Employee> employees = installers.Cast<Employee>().Concat(supervisors.Cast<Employee>()).ToList();
                return View(employees);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Trying to load all employees for index");
                return View("Error", Error.UnknownError());
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {

            var supervisor = await supervisorContext.GetSupervisorById(id);
            if (supervisor.Exists())
            {
                supervisor.Installers = await installerContext.GetInstallersBySupervisor(supervisor);
                var installers = await installerContext.GetAllInstallers();
                ViewBag.installers = installers;
                return PartialView("_SupervisorDetails", supervisor);
            }

            var installer = await installerContext.GetInstallerById(id);
            if (installer.Exists())
            {
                installer.Supervisors = await supervisorContext.GetSupervisorsByInstaller(installer);
                var supervisors = await supervisorContext.GetAllSupervisor();
                ViewBag.supervisors = supervisors;
                return PartialView("_InstallerDetails", installer);
            }

            logger.LogError($"Tried to load installer or supervisor with the id of {id} but found none");
            return View("Error", Error.PersonNotFound());
        }

        public async Task<ActionResult> DeleteInstaller(Guid id)
        {
            var installer = await installerContext.GetInstallerById(id);

            if (!ValidEmployee(id, installer)) return NotFound();

            await installerContext.DeleteInstaller(installer);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteSupervisor(Guid id)
        {
            var supervisor = await supervisorContext.GetSupervisorById(id);
            supervisor.Installers = await installerContext.GetInstallersBySupervisor(supervisor);

            if (!ValidEmployee(id, supervisor)) return View("_Error", Error.PersonNotFound());            

            if (supervisor.Installers.Any())
            {
                logger.LogError($"Tried to delete supervisor of id {id} while stil having installers to supervise");
                return View("_Error", new Error($"Ups, ser ud til der stadig var nogle som {supervisor.Firstname} {supervisor.Lastname} havde ansvar for. Fjern alle som rapportere til {supervisor.Firstname} {supervisor.Lastname}"));
            }

            await supervisorContext.DeleteSupervisor(supervisor);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CreateInstaller()
        {
            var supervisors = await supervisorContext.GetAllSupervisor();

            var supervisorsSelectList = new SelectList(supervisors, "Id", "NameMail");

            ViewBag.Supervisors = supervisorsSelectList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateInstaller(Installer installer)
        {
            try
            {
                installer.Role = Role.Installer;
                await installerContext.CreateInstaller(installer);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Tried to create a new installer");
                return View();
            }
        }

        public ActionResult CreateSupervisor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSupervisor(Supervisor supervisor)
        {
            try
            {
                supervisor.Role = Role.Supervisor;
                await supervisorContext.CreateSupervisor(supervisor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Tried to create a new supervisor");
                return View();
            }
        }

        public async Task<ActionResult> EditInstaller(Guid id)
        {
            var installer = await installerContext.GetInstallerById(id);

            if(!ValidEmployee(id, installer)) return NotFound();            

            return View(installer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInstaller(Installer installer)
        {
            try
            {
                await installerContext.UpdateInstaller(installer);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Tried to update installer of id {installer.Id} but failed");
                return View();
            }
        }

        public async Task<ActionResult> EditSupervisor(Guid id)
        {
            var supervisor = await supervisorContext.GetSupervisorById(id);

            if(!ValidEmployee(id, supervisor)) return NotFound();            

            return View(supervisor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSupervisor(Supervisor supervisor)
        {
            try
            {
                await supervisorContext.UpdateSupervisor(supervisor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Tried to update installer of id {supervisor.Id} but failed");
                return View();
            }
        }

        public async Task<ActionResult> RemoveSupervisorFromInstaller(Guid supervisorId, Guid installerId)
        {
            var installer = await installerContext.GetInstallerById(installerId);
            var supervisor = await supervisorContext.GetSupervisorById(supervisorId);

            if (!ValidEmployee(installerId, installer, supervisorId, supervisor)) return NotFound();

            await installerContext.RemoveSupervisorFromInstaller(installer, supervisor);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RemoveInstallerFromSupervisor(Guid installerId, Guid supervisorId)
        {
            var installer = await installerContext.GetInstallerById(installerId);
            var supervisor = await supervisorContext.GetSupervisorById(supervisorId);

            if (!ValidEmployee(installerId, installer, supervisorId, supervisor)) return NotFound();

            try
            {
                await supervisorContext.RemoveInstallerFromSupervisor(supervisor, installer);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Tried to remove installer with id of {installerId} from supervisor with id of {supervisorId} but failed");
                return View("Error", new Error(ex.Message));
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddInstaller(Guid supervisorId, Guid installerId)
        {
            var installer = await installerContext.GetInstallerById(installerId);
            var supervisor = await supervisorContext.GetSupervisorById(supervisorId);

            if (!ValidEmployee(installerId, installer, supervisorId, supervisor)) return NotFound();

            try
            {
                await installerContext.AddSupervisor(supervisor, installer);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key"))
                {
                    logger.LogError(ex, $"Tried to add  installer with id of {installerId} to supervisor with id of {supervisorId}, but relation alread exists");
                    return View("Error", new Error("Du kan ikke tilføje en person som allerede er der"));
                }
                logger.LogError(ex, $"Something went wrong with adding installer with id of {installerId} to supervisor with id of {supervisorId}");
                return View("Error", new Error(ex.Message));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> AddSupervisor(Guid installerId, Guid supervisorId)
        {
            var installer = await installerContext.GetInstallerById(installerId);
            var supervisor = await supervisorContext.GetSupervisorById(supervisorId);

            if(!ValidEmployee(installerId, installer, supervisorId, supervisor)) return NotFound();            

            try
            {
                await installerContext.AddSupervisor(supervisor, installer);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key")) return View("Error", new Error("Du kan ikke tilføje en person som allerede er der"));
                else return View("Error", new Error(ex.Message));
            }

            return RedirectToAction("Index");
        }

        private bool ValidEmployee(Guid installerId, Installer installer, Guid supervisorId, Supervisor supervisor)
        {
            if (!installer.Exists())
            {
                logger.LogError($"Tried to load installer with id of {installerId} but found none");
            }
            if (!supervisor.Exists())
            {
                logger.LogError($"Tried to load supervisor with id of {supervisorId} but found none");
            }

            return installer.Exists() && supervisor.Exists();
        }

        private bool ValidEmployee(Guid installerId, Installer installer)
        {
            if (!installer.Exists())
            {
                logger.LogError($"Tried to load installer with id of {installerId} but found none");
            }
            return installer.Exists();
        }

        private bool ValidEmployee(Guid supervisorId, Supervisor supervisor)
        {
            if (!supervisor.Exists())
            {
                logger.LogError($"Tried to load supervisor with id of {supervisorId} but found none");
            }
            return supervisor.Exists();
        }
    }
}