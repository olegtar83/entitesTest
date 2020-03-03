import { Component, Inject } from '@angular/core';
import { EntitiesService } from '../services/entities-service';
import { SignalRService } from '../services/signalr.service';
import { IEntity } from '../models/entities';
import { Observable } from 'rxjs/internal/Observable';
import {
  tap, map
} from 'rxjs/operators';
@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css']
})
export class FetchDataComponent {

  public entities$: Observable<IEntity[]>;
  public columns: string[] = [];
  constructor(private entityService: EntitiesService) { 
   
  }


  public ngOnInit(): void {
    this.entities$ = this.entityService.get().pipe(tap(entities => {
      if (entities.length === 0) {
        return;
      }
      const first = entities[0];
      this.columns = Object.keys(first).slice(1);
    }));
  }

  public getTotal(column: number): string {
    return column == 0? 'Sum: ': this.entityService.getTotal(column).toString();
  }

  public cellStyle(num: number) {
    if (num > 0) {
      return {
        'background-color': `rgba(255, 140, 0, ${Math.abs(num)})`,
        'color': num > 0.5 ?'white': 'black'
      };

    }
    if (num === 0) {
      return { 'background-color': `rgb(255, 255, 255)` };
    }
    if (num < 0) {
      return {
        'background-color': `rgba(0, 0, 0, ${Math.abs(num)})`,
        'color': num < -0.5 ?'white': 'black'
      };
    }
  }
}
