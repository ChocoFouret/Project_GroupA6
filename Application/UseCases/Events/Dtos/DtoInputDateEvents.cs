﻿namespace Application.UseCases.Events.Dtos;

public class DtoInputDateEvents
{
    public int? IdAccount { get; set; }
    public int IdSchedule { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}