﻿using System.Linq;
using Betsolutions.Casino.SDK.Internal.TableGames.Backgammon.Repositories;
using Betsolutions.Casino.SDK.TableGames.Backgammon.DTO.Tournament;
using Betsolutions.Casino.SDK.TableGames.Backgammon.Enums;

namespace Betsolutions.Casino.SDK.TableGames.Backgammon.Services
{
    public class BackgammonTournamentService
    {
        private readonly BackgammonTournamentRepository _backgammonTournamentRepository;

        public BackgammonTournamentService(MerchantAuthInfo merchantAuthInfo)
        {
            _backgammonTournamentRepository = new BackgammonTournamentRepository(merchantAuthInfo);
        }

        private string ValidateRequestModel(BackgammonTournamentsFilter filter)
        {
            if (filter.PageIndex < 1)
            {
                return $"invalid {nameof(filter.PageIndex)}";
            }

            if (filter.PageSize < 1)
            {
                return $"invalid {nameof(filter.PageSize)}";
            }

            if (filter.OrderingDirection != null
                && filter.OrderingDirection.ToLowerInvariant() != "asc"
                && filter.OrderingDirection.ToLowerInvariant() != "desc")
            {
                return $"invalid {nameof(filter.OrderingDirection)}";
            }

            return null;
        }

        public GetBackgammonTournamentsResult GetTournaments(BackgammonTournamentsFilter filter)
        {
            var validationErrorMessage = ValidateRequestModel(filter);

            if (null != validationErrorMessage)
            {
                return new GetBackgammonTournamentsResult
                {
                    StatusCode = StatusCodes.InvalidRequest,
                    Message = validationErrorMessage
                };
            }

            var result = _backgammonTournamentRepository.GetTournaments(
                 new Internal.TableGames.Backgammon.DTO.Tournament.TournamentsFilter
                 {
                     EndDateFrom = filter.EndDateFrom,
                     EndDateTo = filter.EndDateTo,
                     GameTypeId = filter.GameTypeId,
                     OrderingDirection = filter.OrderingDirection,
                     OrderingField = filter.OrderingField,
                     PageIndex = filter.PageIndex,
                     PageSize = filter.PageSize,
                     StartDateFrom = filter.StartDateFrom,
                     StartDateTo = filter.StartDateTo,
                     TournamentTypeId = filter.TournamentTypeId
                 });

            if (200 != result.StatusCode)
            {
                return new GetBackgammonTournamentsResult { StatusCode = (StatusCodes)result.StatusCode };
            }

            return new GetBackgammonTournamentsResult
            {
                StatusCode = (StatusCodes)result.StatusCode,
                Data = new BackgammonTournamentPagingResult
                {
                    TotalCount = result.Data.TotalCount,
                    Tournaments = result.Data.Tournaments.Select(i => new BackgammonTournament
                    {
                        TournamentTypeId = i.TournamentTypeId,
                        BetAmount = i.BetAmount,
                        CreateDate = i.CreateDate,
                        EndDate = i.EndDate,
                        FilteredCount = i.FilteredCount,
                        FinalPoint = i.FinalPoint,
                        GameTypeId = i.GameTypeId,
                        Id = i.Id,
                        IsHidden = i.IsHidden,
                        IsNetwork = i.IsNetwork,
                        MaxPlayerCount = i.MaxPlayerCount,
                        MinPlayerCount = i.MinPlayerCount,
                        Prize = i.Prize,
                        Prizes = i.Prizes.Select(p => new BackgammonTournamentPrize
                        {
                            Id = p.Id,
                            Percent = p.Percent
                        }),
                        RegisteredPlayerCount = i.RegisteredPlayerCount,
                        StartDate = i.StartDate,
                        Status = (BackgammonTournamentStatus)i.StatusId,
                        Translations = i.Translations.Select(t => new BackgammonTournamentTranslation
                        {
                            Lang = t.Lang,
                            Name = t.Name
                        })
                    })
                }
            };
        }
    }
}
