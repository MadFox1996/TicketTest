using AutoMapper;
using Microsoft.AspNetCore.Http;
using TaskApi.BLL.Dto;
using TaskApi.BLL.Interfaces;
using TaskApi.DAL.Entities;
using TaskApi.DAL.Interfaces;
using XAct;

namespace TaskApi.BLL.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorage;
        private readonly IFileStorageHelper _fileStorageHelper;

        public TicketService(IUnitOfWork uow, IMapper mapper, IFileStorageService fileStorage, IFileStorageHelper fileStorageHelper)
        {
            _uow = uow;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<TicketDto> GetTaskAsync(int id)
        {
            var ticket = await _uow.Tickets.GetAsync(id);
            if (ticket == null)
            {
                return null;
            }
            var mapperDto = _mapper.Map<TicketDto>(ticket);
            mapperDto.FilePathes
                .AddRange(ticket.TicketFiles
                .Select(x => _fileStorageHelper.GetFileName(ticket.CreatedDate, x.Id, x.FileName)));
            return mapperDto;
        }

        public async Task<int> CreateTaskAsync(TicketDto taskDto)
        {
            // Save file in storage
            var files = await _fileStorage.AddNewFiles(taskDto.Files);

            // Task Creation
            var newTicket = _mapper.Map<Ticket>(taskDto);
            await _uow.Tickets.CreateAsync(newTicket);

            // TaskFileCreation
            files.ForEach(x => x.Ticket = newTicket);
            await _uow.TicketFiles.CreateAsync(files.Select(_mapper.Map<TicketFile>).ToList());

            await _uow.SaveAsync();
            return newTicket.Id;
        }

        public async Task<int> UpdateTaskAsync(TicketDto taskDto)
        {
            var ticket = await _uow.Tickets.GetAsync(taskDto.Id);
            if (ticket != null)
            {
                var ticketFiles = ticket.TicketFiles;
                var filesStoragePathHash = ticketFiles
                    .ToDictionary(x => _fileStorageHelper.GetFileName(ticket.CreatedDate, x.Id, x.FileName), x => x.Hash);

                // Update ticketFile in database
                var updatedFiles = await _fileStorage.AddFiles(taskDto.Files, filesStoragePathHash);

                // Delete files
                var deletedFiles = GetFilesToDelete(taskDto.Files, ticketFiles, filesStoragePathHash);

                var updatedFilesIds = updatedFiles.Select(x => x.Id).ToList();
                var deletedFilesIds = deletedFiles.Select(x => x.Id).ToList();

                var filedIdsToDelete = new List<Guid>();
                filedIdsToDelete.AddRange(updatedFilesIds);
                filedIdsToDelete.AddRange(deletedFilesIds);

                await _fileStorage.RemoveFiles(deletedFiles
                    .Select(x => _fileStorageHelper.GetFileName(ticket.CreatedDate, x.Id, x.FileName))
                    .ToList());

                await _uow.TicketFiles.DeleteAsync(filedIdsToDelete);

                foreach (var updFile in updatedFiles)
                {
                    var ticketFile = _mapper.Map<TicketFile>(updFile);
                    ticketFile.Ticket = ticket;
                    ticket.TicketFiles.Add(ticketFile);
                    await _uow.TicketFiles.CreateAsync(ticketFile);
                }

                ticket.Name = taskDto.Name;
                ticket.Stage = _mapper.Map<DAL.Enum.TicketStage>(taskDto.Stage);

                await _uow.Tickets.UpdateAsync(ticket);
                await _uow.SaveAsync();

                return ticket.Id;
            }
            else
            {
                throw new Exception("Ticket not fount");
            }
        }

        private IEnumerable<TicketFile> GetFilesToDelete(ICollection<IFormFile> files, ICollection<TicketFile> ticketFiles, Dictionary<string, string>? ticketsFileHash)
        {
            var queryFilesHash = files.ToDictionary(x => x.FileName, _fileStorageHelper.CalculateHash);

            // If there are no files in the request, you need to delete everything
            if (queryFilesHash.Count == 0)
            {
                return ticketFiles;
            }

            var res = ticketsFileHash
                 .Where(x => !queryFilesHash.ContainsKey(_fileStorageHelper.GetFileNameFromFilePath(x.Key)) ||
                 (queryFilesHash.ContainsKey(_fileStorageHelper.GetFileNameFromFilePath(x.Key))
                 && x.Value != queryFilesHash[_fileStorageHelper.GetFileNameFromFilePath(x.Key)]))
                 .Select(x => x.Key)
                 .ToList();

            return ticketFiles
                .Where(x => res.Contains(_fileStorageHelper.GetFileName(x.Ticket.CreatedDate, x.Id, x.FileName)));
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var ticket = await _uow.Tickets.GetAsync(id);
            if (ticket != null)
            {
                // Delete related files with ticket
                var filePathes = ticket.TicketFiles
                    .Select(x => _fileStorageHelper.GetFileName(ticket.CreatedDate, x.Id, x.FileName))
                    .ToList();
                await _fileStorage.RemoveFiles(filePathes);

                var ticketToDelete = await _uow.Tickets.DeleteAsync([id]);
                await _uow.SaveAsync();
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _uow.Dispose();
        }
    }
}