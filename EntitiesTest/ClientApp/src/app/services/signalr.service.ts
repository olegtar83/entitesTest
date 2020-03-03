import { Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { IEntity } from '../models/entities';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private connection: HubConnection;
  entityAdded: Subject<IEntity> = new Subject<IEntity>();
  
  constructor( @Inject('BASE_URL') private baseUrl: string) {
     this.connection = new HubConnectionBuilder()
     .withUrl(this.baseUrl +'entityhub')
     .build();
     this.registerOnEvents();
     this.connection.start().catch(err => console.log(err.toString()));
  }
  
  registerOnEvents() {
     this.connection.on('entityAdded', item => {
     console.log('entityAdded');
     this.entityAdded.next(item);
     });
  }}
