import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { SelectivePreloadingStrategyService } from '../lib/selective-preloading-strategy.service';
import { PublicComponent } from './layouts/public/public.component';

const routes: Routes = [
  {
    path: '',
    component: PublicComponent,
    data: { title: 'Public Views' },
    children: [
      {
        path: '**',
        component: PageNotFoundComponent
      }
    ]
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes,
      {
        enableTracing: false, // <-- debugging purposes only
        preloadingStrategy: SelectivePreloadingStrategyService,
      }
    )
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
