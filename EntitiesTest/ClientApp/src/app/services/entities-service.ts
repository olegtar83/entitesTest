import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IEntity } from '../models/entities';
import { map, mergeMap } from 'rxjs/operators';
import { BehaviorSubject } from "rxjs";
import { SignalRService } from "./signalr.service";
@Injectable({ providedIn: 'root' })
export class EntitiesService {

  constructor(private http: HttpClient,
    private signalr: SignalRService,
    @Inject('BASE_URL') private baseUrl: string) { }

  private entities$: BehaviorSubject<any[]> = new BehaviorSubject([]);
  public get() {
    this.http.get<IEntity[]>(this.baseUrl + "api/entity").pipe(map(entities => {
      return entities.map(entity => {
        return this.parse(entity);
      })
    })).subscribe(entities => this.entities$.next(entities));
    this.signalr.entityAdded.subscribe(entity => {
      this.entities$.next([...this.entities$.getValue(), this.parse(entity)]);
    });
    return this.entities$;
  }

  public getTotal(column: number): number {
    return this.entities$.getValue().reduce((sum, current) =>
      sum + current['param' + (column - 1).toString()],
      0);
  }

  private parse(entity: IEntity) {
    for (let i = 0; i < entity.parameters.length; i++) {
      entity['param' + i] = entity.parameters[i];
    }
    delete entity.parameters;
    return entity;
  }
}
