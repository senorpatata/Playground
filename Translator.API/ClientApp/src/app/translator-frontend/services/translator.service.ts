import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { TranslatorResultModel } from "../models/translator-result.model";

@Injectable()
export class TranslatorService {

  availableLanguages: string[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.loadAvailableLanguages();
  }

  loadAvailableLanguages(): void {
    // TODO - Call Backend and retrieve first
    this.http.get<string[]>(this.baseUrl + 'api/translator/availableLangs').subscribe(result => {
      this.availableLanguages = [...result];
    }, error => this.handleError(error));
    this.availableLanguages = ["English", "German", "French"];
  }

  translate(source: string, sourceLang: string, targetLang: string): Promise<TranslatorResultModel> {
    const options = { headers: { 'Content-Type': 'application/json' } };
    const jsonData = JSON.stringify({ source: source, sourceLanguage: sourceLang, targetLanguage: targetLang });
    return this.http.post<TranslatorResultModel>(this.baseUrl + 'api/translator', jsonData, options)
      .toPromise()
      .then((data) => this.handleResult(data))
      .catch((error) => this.handleError(error));
  }

  // Yes, this should be on a common BaseService, and also an injectable httpservices providing simplicity to component's calls
  private handleResult(
    data: any,
  ): Promise<any> {
    
    return Promise.resolve(data);
  }

  private handleError(
    error: any,
  ): Promise<any> {
  
    return Promise.reject(error);
  }
}
