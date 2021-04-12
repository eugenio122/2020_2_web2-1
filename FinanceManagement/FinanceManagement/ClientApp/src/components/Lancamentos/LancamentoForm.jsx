import './Lancamentos.css';

import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Modal, Form, ModalHeader, ModalBody, FormGroup, Label, Input, Row, Col, Button, FormFeedback, Toast } from 'reactstrap';
import { Icon, Dropdown } from 'semantic-ui-react';
import { moneyInputFormat, moneyInputFormatToFloat } from '../../helpers/FnUtils'
import { format } from 'date-fns'

import ContaForm from '../contas/ContaForm'

export default function LancamentoForm(props) {

    const [formData, setFormData] = useState({ tipo: 'despesa', descricao: '', valor: '', data: '', conta: '', categoria: '' });
    const [errorsDataForm, setErrorsDataForm] = useState({ descricao: false, valor: false, data: false, conta: false, categoria: false });
    const [categoriasOptions, setCategoriasOptions] = useState([]);
    const [contasOptions, setContasOptions] = useState([]);
    const [isRepetir, setIsRepetir] = useState(false);

    const [showFormConta, setShowFormConta] = useState(false);

    const [isFixo, setIsFixo] = useState(false);
    const [isParcelado, setIsParcelado] = useState(false);

    const [periodosOptions, setPeriodosOptions] = useState([]);
    const [fixosOptions, setFixosOptions] = useState([]);
    const [periodoFixo, setPeriodoFixo] = useState(4);
    const [periodoParcelado, setPeriodoParcelado] = useState({ qtd: '2', periodo: 2 });

    useEffect(() => {
        getCategorias()
        getContas()
        getPeriodos()
        getFixos()
    }, [])

    useEffect(() => {
        if (props.lancamentoEdit) {
            getLancamentoEdit()
        }
    }, [props.lancamentoEdit])

    function getLancamentoEdit() {
        const lancamento = props.lancamentoEdit
        if (lancamento) {
            setFormData({
                tipo: lancamento.despesaReceita ? 'despesa' : 'receita',
                descricao: lancamento.descricao,
                valor: `${lancamento.despesaReceita ?
                    `-R$${moneyInputFormat(lancamento.tipoLancamento === 'parcelado' ?
                        (lancamento.valor * lancamento.quantidade).toFixed(2) : lancamento.valor.toFixed(2))}`
                    : `R$${moneyInputFormat(lancamento.tipoLancamento === 'parcelado' ?
                        (lancamento.valor * lancamento.quantidade).toFixed(2) : lancamento.valor.toFixed(2))}`}`,
                data: format(new Date(lancamento.data), 'yyyy-MM-dd'),
                categoria: categoriasOptions.find(cat => cat.text === lancamento.categoria) ?
                    categoriasOptions.find(cat => cat.text === lancamento.categoria).id : '',
                conta: contasOptions.find(conta => conta.text === lancamento.conta) ?
                    contasOptions.find(conta => conta.text === lancamento.conta).id : ''
            })
            if (lancamento.tipoLancamento === 'fixo') {
                setIsRepetir(true)
                setIsFixo(true)
                setPeriodoFixo(fixosOptions.find(fixo => fixo.text === lancamento.fixo) ?
                    fixosOptions.find(fixo => fixo.text === lancamento.fixo).id : 4)
            }

            if (lancamento.tipoLancamento === 'parcelado') {
                setIsRepetir(true)
                setIsParcelado(true)
                setPeriodoParcelado({
                    qtd: lancamento.quantidade,
                    periodo: periodosOptions.find(periodo => periodo.text === lancamento.parcelado) ?
                        periodosOptions.find(periodo => periodo.text === lancamento.parcelado).id : 2
                })
            }
        }
    }

    async function getCategorias(newCat) {
        const token = await authService.getAccessToken();
        const response = await fetch('api/categorias', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        const cats = data.map(cat => ({ id: cat.id, text: cat.descCategoria, value: cat.id }))
        setCategoriasOptions(cats)
        setFormData({
            ...formData, categoria: cats.find(cat => cat.text === newCat) ? cats.find(cat => cat.text === newCat).id : ''
        })

    }

    async function getContas() {
        const token = await authService.getAccessToken();
        const response = await fetch('api/contas', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        const contasOpts = data.map(conta => ({ id: conta.id, text: conta.descConta, value: conta.id }))
        setContasOptions(contasOpts)
    }

    const addCategoria = async (e, data) => {
        const token = await authService.getAccessToken();
        const response = await fetch('api/categorias', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
            method: "POST",
            body: JSON.stringify({ descCategoria: data.value, usuarioId: props.user.sub })
        });

        getCategorias(data.value)
    }

    async function getPeriodos() {
        const token = await authService.getAccessToken();
        const response = await fetch('api/periodos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        const periodoOpts = data.map(periodo => ({ id: periodo.id, text: periodo.descPeriodo, value: periodo.id }))
        setPeriodoParcelado({ qtd: '2', periodo: periodoOpts.find(pe => pe.text === 'Meses') ? periodoOpts.find(pe => pe.text === 'Meses').id : 2 })
        setPeriodosOptions(periodoOpts)
    }

    async function getFixos() {
        const token = await authService.getAccessToken();
        const response = await fetch('api/fixos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        const fixosOpts = data.map(fixo => ({ id: fixo.id, text: fixo.descFixo, value: fixo.id }))
        setPeriodoFixo(fixosOpts.find(fix => fix.text === 'Mensal') ? fixosOpts.find(fix => fix.text === 'Mensal').id : 4)
        setFixosOptions(fixosOpts)
    }

    function resetForm() {
        setFormData({ tipo: 'despesa', descricao: '', valor: '', data: '', conta: '', categoria: '', repetir: '' });
        setErrorsDataForm({ descricao: false, valor: false, data: false, conta: false, categoria: false, repetir: '' });
        setIsRepetir(false)
        setIsFixo(false)
        setIsParcelado(false)
        setPeriodoFixo(4)
        setPeriodoParcelado({ qtd: '2', periodo: 2 })
    }

    async function addParcela(e) {
        e.preventDefault()
        const token = await authService.getAccessToken();
        const response = await fetch('api/parcelados', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
            method: "POST",
            body: JSON.stringify({ quantidade: periodoParcelado.qtd, periodoId: periodoParcelado.periodo })
        });

        const data = await response.json();

        submitForm(e, data.id)
    }

    const submitForm = async (e, parceladoId) => {
        e.preventDefault()

        let errors = {};

        if (!formData.descricao.length > 0) {
            errors.descricao = true
        }
        if (!formData.valor.length > 0) {
            errors.valor = true
        }
        if (!formData.data.length > 0) {
            errors.data = true
        }
        if (!formData.conta) {
            errors.conta = true
        }
        if (!formData.categoria) {
            errors.categoria = true
        }

        if (Object.values(errors).some(err => err)) {
            setErrorsDataForm(errors)
            return null
        }

        const payload = {
            despesaReceita: formData.tipo === 'despesa' ? true : false,
            descricao: formData.descricao,
            valor: parceladoId ? moneyInputFormatToFloat(formData.valor) / Number(periodoParcelado.qtd).toFixed(2) : moneyInputFormatToFloat(formData.valor).toFixed(2),
            data: formData.data,
            fixoId: isFixo ? periodoFixo : null,
            parceladoId: parceladoId ? parceladoId : null,
            tipoLancamento: isFixo ? 'fixo' : parceladoId ? 'parcelado' : 'sem repetição',
            usuarioId: props.user.sub
        }

        if (props.lancamentoEdit) {
            const token = await authService.getAccessToken();
            const response = await fetch(`api/lancamentos/${props.lancamentoEdit.id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                method: "PUT",
                body: JSON.stringify(payload)
            });

            if (response.status === 204) {
                props.getLancamentos()
                resetForm()
                props.setShowFormLancamento(false)
            }
        } else {
            const token = await authService.getAccessToken();
            const response = await fetch('api/lancamentos', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (response.status === 201) {
                const data = await response.json();

                lancarContaCategoria(data.id)
            }
        }

    }


    async function lancarContaCategoria(lancamentoId) {
        const token = await authService.getAccessToken();

        const payloadConta = {
            contaId: formData.conta,
            lancamentoId: lancamentoId
        }
        const responseConta = await fetch('api/contaLancamentos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
            method: "POST",
            body: JSON.stringify(payloadConta)
        });

        const payloadCategoria = {
            categoriaId: formData.categoria,
            lancamentoId: lancamentoId
        }

        const responseCategoria = await fetch('api/categoriaLancamentos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
            method: "POST",
            body: JSON.stringify(payloadCategoria)
        });

        if (responseConta.status === 201 && responseCategoria.status === 201) {
            props.getContas()
            props.getLancamentos()
            resetForm()
            props.setShowFormLancamento(false)
        }
    }

    return (
        <div>
            <Modal isOpen={props.showFormLancamento} toggle={() => {
                props.setShowFormLancamento(false)
                resetForm()
                if (props.lancamentoEdit) {
                    props.setLancamentoEdit(null)
                }
            }}>
                <ModalHeader toggle={() => {
                    props.setShowFormLancamento(false)
                    resetForm()
                    if (props.lancamentoEdit) {
                        props.setLancamentoEdit(null)
                    }
                }}>{`${props.lancamentoEdit ? 'Editar lançamento' : 'Novo lançamento'}`}</ModalHeader>
                <ModalBody>
                    <div>
                        <Form onSubmit={isParcelado ? addParcela : submitForm}>
                            <div className='container-type-lancamento' >
                                <div className={`type-lancamento despesa ${formData.tipo === 'despesa' ? 'despesa-active' : ''} `} onClick={() => setFormData({ ...formData, tipo: 'despesa' })}>
                                    <Icon name='arrow down' />
                                    <span>Despesa</span>
                                </div>
                                <div className={`type-lancamento receita ${formData.tipo === 'receita' ? 'receita-active' : ''} `} onClick={() => setFormData({ ...formData, tipo: 'receita' })}>
                                    <Icon name='arrow up' />
                                    <span>Receita</span>
                                </div>
                            </div>
                            <FormGroup>
                                <Label for="form-description">Descrição</Label>
                                <Input
                                    invalid={errorsDataForm.descricao}
                                    name="description"
                                    id="form-description"
                                    value={formData.descricao}
                                    placeholder="Insira a descrição"
                                    onChange={(event) => {
                                        setFormData({ ...formData, descricao: event.target.value })
                                        setErrorsDataForm({ ...errorsDataForm, descricao: false })
                                    }}
                                    onBlur={() => formData.descricao.trim().length < 1 ? setErrorsDataForm({ ...errorsDataForm, descricao: true }) : null} />
                                {errorsDataForm.descricao && <FormFeedback>Informe a descrição</FormFeedback>}
                            </FormGroup>
                            <Row form>
                                <Col>
                                    <FormGroup>
                                        <Label for="form-price">Valor</Label>
                                        <Input
                                            name="price"
                                            invalid={errorsDataForm.valor}
                                            id="form-price"
                                            value={formData.valor}
                                            placeholder="R$ 0,00"
                                            onChange={(event) => {
                                                setErrorsDataForm({ ...errorsDataForm, valor: false })
                                                setFormData({
                                                    ...formData, valor: event.target.value
                                                        ? `${formData.tipo === 'despesa' ? '-R$' : 'R$'
                                                        } ${moneyInputFormat(event.target.value)}`
                                                        : ''
                                                })
                                            }}
                                            onBlur={() => formData.valor.trim().length < 1 ? setErrorsDataForm({ ...errorsDataForm, valor: true }) : null} />
                                        {errorsDataForm.valor && <FormFeedback>Informe o valor</FormFeedback>}
                                    </FormGroup>
                                </Col>
                                <Col>
                                    <FormGroup>
                                        <Label for="form-date">Data</Label>
                                        <Input
                                            type='date'
                                            invalid={errorsDataForm.data}
                                            name="date"
                                            value={formData.data}
                                            id="form-date"
                                            onChange={(event) => {
                                                setFormData({ ...formData, data: event.target.value })
                                                setErrorsDataForm({ ...errorsDataForm, data: false })
                                            }}
                                            onBlur={() => formData.data.trim().length < 1 ? setErrorsDataForm({ ...errorsDataForm, data: true }) : null}
                                        />
                                        {errorsDataForm.data && <FormFeedback>Informe a data</FormFeedback>}
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row form>
                                <Col>
                                    <FormGroup>
                                        <Label for="form-conta">Conta</Label>
                                        <Dropdown
                                            value={formData.conta}
                                            id="form-conta"
                                            selection
                                            options={contasOptions}
                                            search
                                            error={errorsDataForm.conta}
                                            placeholder='Selecione a conta'
                                            onChange={(e, data) => {
                                                setFormData({ ...formData, conta: data.value })
                                                setErrorsDataForm({ ...errorsDataForm, conta: false })
                                            }} />
                                        {errorsDataForm.conta && <div className='invalid-form'>Informe a conta</div>}
                                    </FormGroup>
                                </Col>
                                <Col>
                                    <FormGroup>
                                        <Label for="form-categoria">Categoria</Label>
                                        <Dropdown
                                            value={formData.categoria}
                                            id="form-categoria"
                                            selection
                                            options={categoriasOptions}
                                            search
                                            error={errorsDataForm.categoria}
                                            placeholder='Selecione a categoria'
                                            allowAdditions={true}
                                            onAddItem={addCategoria}
                                            onChange={(e, data) => {
                                                setFormData({ ...formData, categoria: data.value })
                                                setErrorsDataForm({ ...errorsDataForm, categoria: false })
                                            }} />
                                        {errorsDataForm.categoria && <div className='invalid-form'>Informe a categoria</div>}
                                    </FormGroup>
                                </Col>
                            </Row>
                            <FormGroup>
                                <Button
                                    type='button'
                                    outline={!isRepetir}
                                    color='info'
                                    style={{ width: '100%' }}
                                    onClick={() => setIsRepetir(!isRepetir)}
                                >
                                    <Icon name='history' />
                                    Repetir
                                </Button>
                                {isRepetir &&
                                    <>
                                        <FormGroup style={{ marginTop: 10 }} check>
                                            <Label check>
                                                <Input className='boxestilizado' type="checkbox" name="fixo" checked={isFixo} onChange={() => {
                                                    setIsFixo(!isFixo)
                                                    setIsParcelado(false)
                                                }} />
                                                É um lançamento fixo
                                            </Label>
                                        </FormGroup>
                                        <FormGroup check>
                                            <Label check>
                                                <Input type="checkbox" name="parcelado" checked={isParcelado} onChange={() => {
                                                    setIsFixo(false)
                                                    setIsParcelado(!isParcelado)
                                                }} />{' '}
                                                É um lançamento parcelado em
                                            </Label>
                                        </FormGroup>

                                        {isParcelado &&
                                            <Row form>
                                                <Col>
                                                    <Label for="form-qtd">Quantidade de parcelas</Label>
                                                    <Input
                                                        type='number'
                                                        name="qtd"
                                                        value={periodoParcelado.qtd}
                                                        id="form-qtd"
                                                        min={2}
                                                        onChange={(event) => {
                                                            setPeriodoParcelado({ ...periodoParcelado, qtd: event.target.value })
                                                        }}
                                                    />
                                                </Col>
                                                <Col>
                                                    <Label for="form-periodo">Periodo</Label>
                                                    <Dropdown
                                                        value={periodoParcelado.periodo}
                                                        id="form-periodo"
                                                        selection
                                                        options={periodosOptions}
                                                        search
                                                        onChange={(e, data) => {
                                                            setPeriodoParcelado({ ...periodoParcelado, periodo: data.value })
                                                        }} />
                                                </Col>
                                            </Row>
                                        }

                                        {isFixo ?
                                            <FormGroup>
                                                <Label for="form-periodo">Periodo</Label>
                                                <Dropdown
                                                    value={periodoFixo}
                                                    id="form-periodo"
                                                    selection
                                                    options={fixosOptions}
                                                    search
                                                    onChange={(e, data) => {
                                                        setPeriodoFixo(data.value)
                                                    }} />
                                            </FormGroup>
                                            : null
                                        }
                                    </>
                                }
                            </FormGroup>
                            <FormGroup>
                                <Button
                                    type='submit'
                                    color='success'
                                    style={{ width: '100%' }}
                                >
                                    {`${props.lancamentoEdit ? 'Editar' : 'Salvar'}`}
                                </Button>
                            </FormGroup>
                        </Form>
                    </div>
                </ModalBody >
            </Modal >
        </div >
    )
}