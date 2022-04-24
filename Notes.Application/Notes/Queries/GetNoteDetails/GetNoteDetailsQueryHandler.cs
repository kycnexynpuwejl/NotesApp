using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
{
    private INotesDbContext _dbContext;
    private IMapper _mapper;

    public GetNoteDetailsQueryHandler(INotesDbContext dbcontext, IMapper mapper) =>
        (_dbContext, _mapper) = (dbcontext, mapper);
    
    public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Notes
            .FirstOrDefaultAsync(note =>
                note.Id == request.Id, cancellationToken);

        if (entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Note), request.Id);
        }

        return _mapper.Map<NoteDetailsVm>(entity);
    }
}