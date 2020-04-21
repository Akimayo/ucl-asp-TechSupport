import React, { FormEvent } from 'react';
import { Card, CardTitle, CardText, CardActions, Button, Textfield, Chip, Icon } from 'react-mdl';

interface IProps { }
interface IState {
  categories: Array<any>;
  selectedCategory?: number;
  products: Array<any>;
  selectedProduct?: number;
  error?: string;
  success?: string;
  defaultState?: any;
  reportMessage?: string;
  email?: string;
  hasFile: boolean;
  fileName: string;
}

export default class Report extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      categories: [{ name: 'Loading Product Categories...', sys: true }],
      products: [],
      selectedCategory: undefined,
      selectedProduct: undefined,
      error: undefined,
      success: undefined,
      defaultState: undefined,
      reportMessage: undefined,
      email: undefined,
      fileName: 'Add attachment (optional)',
      hasFile: false
    };
  }

  componentDidMount() {
    this.setState(s => {
      return { defaultState: s };
    });
    this.fetchCategories();
  }

  render() {
    return (
      <form onSubmit={evt => this.formSubmitAjax(evt)}>
        <Card shadow={4} className='card-center'>
          <CardTitle>Report an Issue</CardTitle>
          <CardText className='mdl-grid'>
            <div className={'mdl-textfield mdl-js-textfield mdl-textfield--floating-label mdl-cell mdl-cell--5-col ' + (this.state.selectedCategory ? 'is-dirty' : 'is-invalid')} id='select-category-wrapper'>
              <select name='categoryId' id='select-category' className='mdl-textfield__input' value={this.state.selectedCategory || -1} required onChange={evt => this.selectCategoryChanged(evt)}>
                {this.state.categories.map((opt: any) => (<option value={opt.sys ? -1 : opt.id} disabled={opt.sys} style={opt.sys && { display: 'none' }} key={opt.id || -1}>{opt.name}</option>))}
              </select>
              <label htmlFor='select-category' className='mdl-textfield__label'>Category</label>
            </div>
            <Icon name='chevron_right' className='mdl-cell mdl-cell--2-col' style={{ verticalAlign: 'middle', textAlign: 'center', padding: '24px 0' }} />
            <div className={`mdl-textfield mdl-js-textfield mdl-textfield--floating-label mdl-cell mdl-cell--5-col ${this.state.selectedProduct ? 'is-dirty' : 'is-invalid'} ${this.state.selectedCategory ? '' : 'is-disabled'}`}>
              <select name='productId' id='select-product' className='mdl-textfield__input' value={this.state.selectedProduct || -1} required disabled={!this.state.selectedCategory} onChange={evt => this.selectProductChanged(evt)}>
                {this.state.products.map((opt: any) => (<option value={opt.sys ? -1 : opt.id} disabled={opt.sys} style={opt.sys && { display: 'none' }} key={opt.id || -1}>{opt.name}</option>))}
              </select>
              <label htmlFor='select-product' className='mdl-textfield__label'>Product</label>
            </div>
            <div className={`mdl-textfield mdl-js-textfield mdl-textfield--floating-label mdl-cell mdl-cell--12-col ${this.state.reportMessage ? 'is-dirty' : 'is-invalid'} ${this.state.selectedProduct ? '' : 'is-disabled'}`}>
              <textarea name='report' id='textarea-report' rows={10} className='mdl-textfield__input' maxLength={500} required disabled={!this.state.selectedProduct} onInput={evt => this.setReportMessage(evt)} value={this.state.reportMessage || ''}></textarea>
              <label htmlFor='textarea-report' className='mdl-textfield__label'>Report Message</label>
            </div>
            <Textfield name='email' label='E-mail Address' required={true} type='email' floatingLabel={true} disabled={!this.state.selectedProduct} className={`mdl-cell mdl-cell--4-col ${this.state.email ? 'is-dirty' : 'is-invalid'} ${this.state.selectedProduct ? '' : 'is-disabled'}`} onInput={evt => this.setEmail(evt)} value={this.state.email} />
            <label className='mdl-button mdl-js-button mdl-js-ripple-effect mdl-cell mdl-cel--3-col' htmlFor='file-attach' id='label-file-upload'>
              <input type='file' style={{ display: 'none' }} id='file-attach' name='attachment' onChange={evt => this.fileChanged(evt)} disabled={!this.state.selectedProduct} />
              <Icon name='attach_file' />
              {this.state.fileName}
            </label>
            <div className='mdl-cell mdl-cell--12-col'>
              <Chip className='mdl-color--accent mdl-color-text--accent-contrast mdl-cell mdl-cell--12-col mdl-shadow--2dp' style={{ display: this.state.error ? 'inline-block' : 'none' }}>{this.state.error}</Chip>
              <Chip className='mdl-color--primary mdl-color-text--primary-contrast mdl-cell mdl-cell--12-col mdl-shadow--2dp' style={{ display: this.state.success ? 'inline-block' : 'none' }}>{this.state.success}</Chip>
            </div>
          </CardText>
          <CardActions>
            <Button type='submit' colored={true} raised={true} ripple={true}>Submit</Button>
          </CardActions>
        </Card>
      </form>
    );
  }
  private fetchCategories(): void {
    document.getElementById('label-file-upload')?.setAttribute('disabled', 'disabled');
    fetch('https://localhost:5001/api/Reporting/Categories', { mode: 'cors' })
      .then((res: Response) => res.json())
      .then((json: any) => this.setState(() => {
        return { categories: [{ name: '', sys: true }, ...json] };
      }, () => document.getElementById('select-category-wrapper')?.classList.remove('is-dirty')))
      .catch(() => this.setState(() => {
        return { categories: [{ name: 'Couldn\'t fetch product categories. Please try again later.', sys: true }] };
      }));
  }
  private fetchProducts(): void {
    document.getElementById('label-file-upload')?.setAttribute('disabled', 'disabled');
    fetch('https://localhost:5001/api/Reporting/Products?category=' + this.state.selectedCategory, { mode: 'cors' })
      .then((res: Response) => res.json())
      .then((json: any) => this.setState(() => {
        return { products: [{ name: '', sys: true }, ...json] };
      }))
      .catch(() => this.setState(() => {
        return { products: [{ name: 'Couldn\'t fetch products. Please try again later.', sys: true }] };
      }));
  }
  //#region Event Handlers
  private selectCategoryChanged(event: React.FormEvent<HTMLSelectElement>): void {
    const val: number = parseInt(event.currentTarget.value);
    this.setState(() => {
      return { selectedCategory: val, products: [{ name: 'Loading Products...', sys: true }], selectedProduct: undefined };
    }, this.fetchProducts);
  }
  private selectProductChanged(event: FormEvent<HTMLSelectElement>): void {
    const val: number = parseInt(event.currentTarget.value);
    this.setState(() => {
      return { selectedProduct: val };
    }, () => document.getElementById('label-file-upload')?.removeAttribute('disabled'));
  }
  private setReportMessage(event: FormEvent<HTMLTextAreaElement>): void {
    const val: string = event.currentTarget.value;
    this.setState(() => {
      return { reportMessage: val };
    })
  }
  private setEmail(event: FormEvent<HTMLInputElement>): void {
    const val: string = event.currentTarget.value;
    this.setState(() => {
      return { email: val };
    });
  }
  private fileChanged(event: FormEvent<HTMLInputElement>): void {
    const val = event.currentTarget.files?.item(0);
    val && this.setState(() => {
      return { fileName: val.name, hasFile: true };
    });
  }
  private formSubmitAjax(event: FormEvent<HTMLFormElement>): void {
    event.preventDefault();
    (this.state.error || this.state.success) && this.setState(() => {
      return { error: undefined, success: undefined };
    });
    const dataset: FormData = new FormData();
    this.state.selectedProduct && dataset.append('ProductId', this.state.selectedProduct.toString());
    this.state.reportMessage && dataset.append('Message', this.state.reportMessage);
    this.state.email && dataset.append('Email', this.state.email);
    const fileSelect: FileList | null = (document.getElementById('file-attach') as HTMLInputElement).files;
    this.state.hasFile && fileSelect && fileSelect.length > 0 && dataset.append('Attachment', fileSelect.item(0) as Blob);
    fetch('https://localhost:5001/api/Reporting/Report', {
      method: 'POST',
      mode: 'cors',
      cache: 'no-cache',
      credentials: 'same-origin',
      headers: { 'Accept': 'application/json' },
      redirect: 'follow',
      referrerPolicy: 'no-referrer',
      body: dataset
    }).then((res: Response) => res.json())
      .then((json: any) => {
        if (!json.success) throw new Error();
        else this.setState(s => {
          return { ...s.defaultState, defaultState: s.defaultState, success: 'Your report has been successfully saved' };
        }, () => {
          this.fetchCategories();
          setTimeout(() => this.setState(() => { return { success: undefined } }), 5000);
        });
      })
      .catch(() => {
        this.setState(() => {
          return { error: 'Failed sending report. Please try again later.' };
        }, () => setTimeout(() => this.setState(() => { return { error: undefined } }), 5000));
      });
  }
  //#endregion
}