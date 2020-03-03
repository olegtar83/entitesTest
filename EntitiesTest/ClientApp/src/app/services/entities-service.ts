import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs/internal/Observable";
import { HttpClient } from "@angular/common/http";
import { IEntity } from '../models/entities';
import { map } from 'rxjs/operators';
import { from } from "rxjs";
@Injectable({ providedIn: 'root' })
export class EntitiesService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  public get(): Observable<any[]> {
    return this.http.get<IEntity[]>(this.baseUrl + "api/entity").pipe(map(entities => {
      return entities.map(entity => {
        for (let i = 0; i < entity.parameters.length; i++) {
          entity['param' + i] = entity.parameters[i];
        }
        delete entity.parameters;
        return entity;
      });
    }));
  }
}


