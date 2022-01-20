import { Component } from '@angular/core';
import { TranslatorModel } from './models/translator.model';
import { TranslatorService } from './services/translator.service';

@Component({
  selector: 'translator-frontend',
  templateUrl: './translator-frontend.component.html',
})
export class TranslatorFrontendComponent {

  model: TranslatorModel = new TranslatorModel();

  constructor(private service: TranslatorService) {
    this.model.availableLanguages = service.availableLanguages;
    this.model.targetText = '';
    this.model.originText = 'Hello, my name is Angel Castle'; // Just hard-coded for fun
    if (this.model.availableLanguages && this.model.availableLanguages.length) {
      this.model.originLang = this.model.availableLanguages[0];
      this.model.targetLang = this.model.availableLanguages[0];
    }
  }

  doReverse(): void {
    var newModel = new TranslatorModel();
    newModel.availableLanguages = this.model.availableLanguages;
    newModel.targetLang = this.model.originLang;
    newModel.originLang = this.model.targetLang;
    newModel.originText = this.model.targetText;
    newModel.targetText = this.model.originText;
    this.model = newModel;
  }

  async doTranslation(): Promise<void> {
    const result = await this.service.translate(this.model.originText, this.model.originLang, this.model.targetLang);

    if (!result.success) {
      alert(result.errorCode);
      return;
    }

    this.model.targetText = result.result;
  }
}
