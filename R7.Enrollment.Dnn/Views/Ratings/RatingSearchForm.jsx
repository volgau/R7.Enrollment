export default class RatingSearchForm extends React.Component {
    constructor (props) {
        super (props);
        this.refs.personalNumber = React.createRef ();
        this.refs.snils = React.createRef ();
        this.handleSubmit = this.handleSubmit.bind (this);
        this.doSubmit = this.doSubmit.bind (this);
        this.handleNoSnilsChange = this.handleNoSnilsChange.bind (this);
        this.state = this.createDefaultState ();
    }

    createDefaultState () {
        return {
            isError: false,
            invalidSnils: false,
            invalidPersonalNumber: false,
            requestDone: false,
            requestInProgress: false,
            lists: []
        };
    }

    handleSubmit (e) {
        e.preventDefault ();
        const formData = new FormData (e.target);
        const data = {
            snils: formData.get ("snils"),
            personalNumber: formData.get ("personalNumber")
        };

        // validate form
        const invalidSnils = !this.validateSnils (data.snils, this.props.noSnils);
        const invalidPersonalNumber = !this.validatePersonalNumber (data.personalNumber, this.props.noSnils);
        if (invalidSnils || invalidPersonalNumber) {
            const newState = this.createDefaultState ();
            newState.invalidSnils = invalidSnils;
            newState.invalidPersonalNumber = invalidPersonalNumber;
            this.setState (newState);
            return;
        }

        // remove potentially invalid data before send
        if (this.props.noSnils) {
            data.snils = "";
        }
        else {
            data.personalNumber = "";
        }

        setTimeout (this.doSubmit, 750, data);

        const newState = this.createDefaultState ();
        newState.requestInProgress = true;
        this.setState (newState);
    }

    doSubmit (data) {
        this.props.service.getRatingListsByEntrant (data,
            (results) => {
                const newState = this.createDefaultState ();
                newState.requestInProgress = false;
                newState.requestDone = true;
                newState.lists = results;
                this.setState (newState);
            },
            (xhr, status, err) => {
                console.log (xhr);
                const newState = this.createDefaultState ();
                newState.requestInProgress = false;
                newState.requestDone = true;
                newState.isError = true;
                this.setState (newState);
            }
        );
    }

    validateSnils (snils, noSnils) {
        if (noSnils) {
            return true;
        }
        if (typeof (snils) === "undefined" || snils === null || snils.length === 0) {
            return false;
        }
        const snilsStripped = snils.replace (/[^\d]/g, "");
        if (snilsStripped.length !== 11) {
            return false;
        }
        return true;
    }

    validatePersonalNumber (personalNumber, noSnils) {
        if (!noSnils) {
            return true;
        }
        if (typeof (personalNumber) === "undefined" || personalNumber === null || personalNumber.length === 0) {
            return false;
        }
        return true;
    }

    handleNoSnilsChange (e) {
        this.props.onNoSnilsChange (e.target.checked);
    }

    render () {
        return (
            <div>
                {this.renderForm ()}
                <RatingSearchResults requestDone={this.state.requestDone} lists={this.state.lists} isError={this.state.isError} />
            </div>
        );
    }

    renderForm () {
        return (
            <>
            <h3>Поиск по спискам</h3>
            <form onSubmit={this.handleSubmit} className="mb-3">
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_snils">СНИЛС</label>
                    <input type="search" name="snils" id="enrRatingSearch_snils" ref={this.refs.snils} maxLength="64"
                           className={"form-control " + (this.state.invalidSnils ? "is-invalid" : "")}
                           placeholder="Введите СНИЛС (11 цифр)"
                           disabled={this.props.noSnils} />
                    {this.state.invalidSnils ? <div className="invalid-feedback">Введите СНИЛС (11 цифр)</div> : null}
                </div>
                <div className="form-group">
                    <div className="form-check">
                        <input className="form-check-input" type="checkbox" id="enrRatingSearch_noSnils" onClick={this.handleNoSnilsChange} checked={this.props.noSnils} />
                        <label className="form-check-label" htmlFor="enrRatingSearch_noSnils">У меня нет СНИЛС!</label>
                    </div>
                </div>
                <div className={"form-group " + (!this.props.noSnils ? "d-none" : "")}>
                    <label htmlFor="enrRatingSearch_personalNumber">Личный номер абитуриента</label>
                    <input type="number" min="2100000" max="2199999" name="personalNumber" id="enrRatingSearch_personalNumber"
                           ref={this.refs.personalNumber}
                           className={"form-control " + (this.state.invalidPersonalNumber? "is-invalid" : "")}
                           placeholder="Введите личный номер в формате 21XXXXX (7 цифр)" />
                    {this.state.invalidPersonalNumber ? <div className="invalid-feedback">Введите личный номер в формате 21XXXXX (7 цифр)</div> : null}
                </div>
                <button type="submit" className="btn btn-primary" disabled={this.state.requestInProgress}>
                    {this.state.requestInProgress ? <i className="fas fa-circle-notch fa-spin mr-2"></i> : null}
                    Найти меня в списках!
                </button>
            </form>
            </>
        );
    }

    componentDidMount () {
        // this.refs.snils.current.value = "000-000-000-00";
        // this.refs.personalNumber.current.value = "2100000";
    }
}

class RatingSearchResults extends React.Component {
    constructor (props) {
        super (props);
    }

    render () {
        if (!this.props.requestDone) {
            return null;
        }
        if (this.props.lists.length > 0) {
            return this.props.lists.map (list => (
                <div>
                    <hr />
                    <div dangerouslySetInnerHTML={{__html: list.Html}} />
                </div>
            ));
        }
        if (!this.props.isError) {
            return (
                <p className="alert alert-warning">По вашему запросу ничего не найдено!</p>
            );
        }
        return (
            <p className="alert alert-danger">Ой, что-то пошло не так! Перезагрузите страницу и попробуйте снова.</p>
        )
    }
}

